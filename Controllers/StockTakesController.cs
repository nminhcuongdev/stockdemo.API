using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Data;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.LocationRepository;
using StockDemo.API.Repositories.StockRepository;
using StockDemo.API.Repositories.StockTakeRepository;
using StockDemo.API.Services;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockTakesController : ControllerBase
    {
        private readonly IStockTakeRepository stockTakeRepository;
        private readonly IStockRepository stockRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;
        private readonly StockDemoDbContext dbContext;
        private readonly LowStockAlertService lowStockAlertService;

        public StockTakesController(
            IStockTakeRepository stockTakeRepository,
            IStockRepository stockRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            StockDemoDbContext dbContext,
            LowStockAlertService lowStockAlertService)
        {
            this.stockTakeRepository = stockTakeRepository;
            this.stockRepository = stockRepository;
            this.locationRepository = locationRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.lowStockAlertService = lowStockAlertService;
        }

        // GET: api/stocktakes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await stockTakeRepository.GetAllWithDetailsAsync();
            var dtos = mapper.Map<List<StockTakeDto>>(sessions);
            return Ok(ApiResponse<List<StockTakeDto>>.SuccessResult(dtos, "Lấy lịch sử kiểm kê thành công"));
        }

        // GET: api/stocktakes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var session = await stockTakeRepository.GetWithDetailsAsync(id);
            if (session == null)
            {
                return NotFound(ApiResponse<StockTakeDto>.ErrorResult("Không tìm thấy phiên kiểm kê"));
            }

            var dto = mapper.Map<StockTakeDto>(session);
            return Ok(ApiResponse<StockTakeDto>.SuccessResult(dto, "Lấy phiên kiểm kê thành công"));
        }

        // POST: api/stocktakes  -> creates a session and computes variances (does NOT adjust stock yet).
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockTakeDto createDto)
        {
            var location = await locationRepository.GetByIdAsync(createDto.LocationId);
            if (location == null)
            {
                return NotFound(ApiResponse<StockTakeDto>.ErrorResult("Vị trí kiểm kê không tồn tại"));
            }

            var session = new StockTake
            {
                LocationId = createDto.LocationId,
                Note = createDto.Note,
                CreatedBy = createDto.CreatedBy,
                Status = "InProgress",
                CreatedDate = DateTime.Now,
                Items = new List<StockTakeItem>()
            };

            foreach (var line in createDto.Items)
            {
                var stock = await stockRepository.GetByProductAndLocationAsync(line.ProductId, createDto.LocationId);
                var systemQty = stock?.Quantity ?? 0;
                session.Items.Add(new StockTakeItem
                {
                    ProductId = line.ProductId,
                    SystemQuantity = systemQty,
                    CountedQuantity = line.CountedQuantity,
                    Variance = line.CountedQuantity - systemQty
                });
            }

            var created = await stockTakeRepository.AddAsync(session);
            var dto = mapper.Map<StockTakeDto>(await stockTakeRepository.GetWithDetailsAsync(created.StockTakeId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.StockTakeId },
                ApiResponse<StockTakeDto>.SuccessResult(dto, "Tạo phiên kiểm kê thành công")
            );
        }

        // POST: api/stocktakes/{id}/complete  -> reconciles stock to the counted quantities.
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete([FromRoute] int id)
        {
            var session = await stockTakeRepository.GetWithDetailsAsync(id);
            if (session == null)
            {
                return NotFound(ApiResponse<StockTakeDto>.ErrorResult("Không tìm thấy phiên kiểm kê"));
            }

            if (session.Status == "Completed")
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Phiên kiểm kê đã được hoàn tất"));
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in session.Items)
                {
                    var stock = await stockRepository.GetByProductAndLocationAsync(item.ProductId, session.LocationId);
                    if (stock != null)
                    {
                        await stockRepository.SetQuantityAsync(stock.StockId, item.CountedQuantity);
                    }
                    else if (item.CountedQuantity > 0)
                    {
                        await stockRepository.AddAsync(new Stock
                        {
                            ProductId = item.ProductId,
                            LocationId = session.LocationId,
                            Quantity = item.CountedQuantity,
                            QRCode = $"STK-{item.ProductId}-{session.LocationId}",
                            LastUpdated = DateTime.Now
                        });
                    }
                }

                session.Status = "Completed";
                session.CompletedDate = DateTime.Now;
                await stockTakeRepository.UpdateAsync(session);

                await transaction.CommitAsync();

                // Reconciliation may have reduced stock below reorder level; notify (non-blocking).
                lowStockAlertService.QueueCheck();

                var dto = mapper.Map<StockTakeDto>(await stockTakeRepository.GetWithDetailsAsync(id));
                return Ok(ApiResponse<StockTakeDto>.SuccessResult(dto, "Đối chiếu & điều chỉnh tồn kho thành công"));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ApiResponse<object>.ErrorResult("Đối chiếu thất bại, đã hoàn tác thay đổi"));
            }
        }
    }
}
