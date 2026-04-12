using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.StockInRepository;
using System.Linq;
using System.Threading.Tasks;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockInsController : ControllerBase
    {
        private readonly IStockInRepository stockInRepository;
        private readonly IMapper mapper;

        public StockInsController(IStockInRepository stockInRepository, IMapper mapper)
        {
            this.stockInRepository = stockInRepository;
            this.mapper = mapper;
        }

        // GET: api/stockins
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var pagedStockIns = await stockInRepository.GetAllStockInsWithDetailsAsync(
                filterOn, filterQuery, sortBy, sortOrder, pageNumber, pageSize);

            var dtoItems = mapper.Map<List<StockInDto>>(pagedStockIns.Items);

            var pagedResultDto = new PagedResult<StockInDto>
            {
                Items = dtoItems,
                PageNumber = pagedStockIns.PageNumber,
                PageSize = pagedStockIns.PageSize,
                TotalCount = pagedStockIns.TotalCount
            };

            return Ok(ApiResponse<PagedResult<StockInDto>>.SuccessResult(pagedResultDto, "Lấy danh sách phiếu nhập thành công"));
        }

        // GET: api/stockins/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stockIn = await stockInRepository.GetStockInWithDetailsAsync(id);

            if (stockIn == null)
            {
                return NotFound(ApiResponse<StockInDto>.ErrorResult("Không tìm thấy phiếu nhập"));
            }

            var stockInDto = mapper.Map<StockInDto>(stockIn);
            return Ok(ApiResponse<StockInDto>.SuccessResult(stockInDto, "Lấy thông tin phiếu nhập thành công"));
        }


        // GET: api/stockins/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId)
        {
            var stockIns = await stockInRepository.GetByProductAsync(productId);
            var stockInDtos = mapper.Map<List<StockInDto>>(stockIns);

            return Ok(ApiResponse<List<StockInDto>>.SuccessResult(stockInDtos, "Lấy phiếu nhập theo sản phẩm thành công"));
        }


        // GET: api/stockins/daterange?startDate=...&endDate=...
        [HttpGet("daterange")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stockIns = await stockInRepository.GetByDateRangeAsync(startDate, endDate);
            var stockInDtos = mapper.Map<List<StockInDto>>(stockIns);

            return Ok(ApiResponse<List<StockInDto>>.SuccessResult(stockInDtos, "Lấy phiếu nhập theo khoảng thời gian thành công"));
        }

        // POST: api/stockins
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockInDto createStockInDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockInDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var stockIn = mapper.Map<StockIn>(createStockInDto);
            stockIn.CreatedDate = DateTime.Now;

            var createdStockIn = await stockInRepository.AddAsync(stockIn);
            var stockInDto = mapper.Map<StockInDto>(await stockInRepository.GetStockInWithDetailsAsync(createdStockIn.StockInId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdStockIn.StockInId },
                ApiResponse<StockInDto>.SuccessResult(stockInDto, "Tạo phiếu nhập thành công")
            );
        }

        // PUT: api/stockins/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockInDto updateStockInDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockInDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var stockIn = await stockInRepository.GetByIdAsync(id);

            if (stockIn == null)
            {
                return NotFound(ApiResponse<StockInDto>.ErrorResult("Không tìm thấy phiếu nhập"));
            }


            if (updateStockInDto.ProductId.HasValue)
                stockIn.ProductId = updateStockInDto.ProductId.Value;

            if (updateStockInDto.LocationId.HasValue)
                stockIn.LocationId = updateStockInDto.LocationId.Value;

            if (updateStockInDto.Quantity.HasValue)
                stockIn.Quantity = updateStockInDto.Quantity.Value;

            if (!string.IsNullOrEmpty(updateStockInDto.QRCode))
                stockIn.QRCode = updateStockInDto.QRCode;

            await stockInRepository.UpdateAsync(stockIn);
            var stockInDto = mapper.Map<StockInDto>(await stockInRepository.GetStockInWithDetailsAsync(id));

            return Ok(ApiResponse<StockInDto>.SuccessResult(stockInDto, "Cập nhật phiếu nhập thành công"));
        }

        // DELETE: api/stockins/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var success = await stockInRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy phiếu nhập"));
            }

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa phiếu nhập thành công"));
        }
    }
}
