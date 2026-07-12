using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Data;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.LocationRepository;
using StockDemo.API.Repositories.StockRepository;
using StockDemo.API.Repositories.StockTransferRepository;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockTransfersController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly IStockTransferRepository stockTransferRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;
        private readonly StockDemoDbContext dbContext;

        public StockTransfersController(
            IStockRepository stockRepository,
            IStockTransferRepository stockTransferRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            StockDemoDbContext dbContext)
        {
            this.stockRepository = stockRepository;
            this.stockTransferRepository = stockTransferRepository;
            this.locationRepository = locationRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        // GET: api/stocktransfers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transfers = await stockTransferRepository.GetAllTransfersWithDetailsAsync();
            var dtos = mapper.Map<List<StockTransferDto>>(transfers);
            return Ok(ApiResponse<List<StockTransferDto>>.SuccessResult(dtos, "Lấy lịch sử chuyển kho thành công"));
        }

        // GET: api/stocktransfers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var transfer = await stockTransferRepository.GetTransferWithDetailsAsync(id);
            if (transfer == null)
            {
                return NotFound(ApiResponse<StockTransferDto>.ErrorResult("Không tìm thấy phiếu chuyển kho"));
            }

            var dto = mapper.Map<StockTransferDto>(transfer);
            return Ok(ApiResponse<StockTransferDto>.SuccessResult(dto, "Lấy thông tin chuyển kho thành công"));
        }

        // POST: api/stocktransfers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransferStockDto transferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockTransferDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var source = await stockRepository.GetByIdAsync(transferDto.SourceStockId);
            if (source == null)
            {
                return NotFound(ApiResponse<StockTransferDto>.ErrorResult("Không tìm thấy tồn kho nguồn"));
            }

            var toLocation = await locationRepository.GetByIdAsync(transferDto.ToLocationId);
            if (toLocation == null)
            {
                return NotFound(ApiResponse<StockTransferDto>.ErrorResult("Vị trí đích không tồn tại"));
            }

            if (transferDto.ToLocationId == source.LocationId)
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Vị trí đích phải khác vị trí nguồn"));
            }

            if (transferDto.Quantity <= 0)
            {
                return BadRequest(ApiResponse<object>.ErrorResult("Số lượng chuyển phải lớn hơn 0"));
            }

            if (transferDto.Quantity > source.Quantity)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(
                    $"Số lượng chuyển ({transferDto.Quantity}) vượt quá tồn kho nguồn ({source.Quantity})"));
            }

            var productId = source.ProductId;
            var fromLocationId = source.LocationId;
            var qrCode = source.QRCode;

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Trừ tồn kho ở vị trí nguồn.
                await stockRepository.DecreaseQuantityAsync(source.StockId, transferDto.Quantity);

                // Cộng vào vị trí đích: gộp nếu đã có tồn cùng sản phẩm, ngược lại tạo mới.
                var destination = await stockRepository.GetByProductAndLocationAsync(productId, transferDto.ToLocationId);
                if (destination != null)
                {
                    await stockRepository.IncreaseQuantityAsync(destination.StockId, transferDto.Quantity);
                }
                else
                {
                    var newStock = new Stock
                    {
                        ProductId = productId,
                        LocationId = transferDto.ToLocationId,
                        Quantity = transferDto.Quantity,
                        QRCode = qrCode,
                        LastUpdated = DateTime.Now
                    };
                    await stockRepository.AddAsync(newStock);
                }

                // Ghi log chuyển kho.
                var transfer = new StockTransfer
                {
                    ProductId = productId,
                    FromLocationId = fromLocationId,
                    ToLocationId = transferDto.ToLocationId,
                    Quantity = transferDto.Quantity,
                    QRCode = qrCode,
                    CreatedBy = transferDto.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                var created = await stockTransferRepository.AddAsync(transfer);

                await transaction.CommitAsync();

                var dto = mapper.Map<StockTransferDto>(await stockTransferRepository.GetTransferWithDetailsAsync(created.StockTransferId));
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.StockTransferId },
                    ApiResponse<StockTransferDto>.SuccessResult(dto, "Chuyển kho thành công")
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ApiResponse<object>.ErrorResult("Chuyển kho thất bại, đã hoàn tác thay đổi"));
            }
        }
    }
}
