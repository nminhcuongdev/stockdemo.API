using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.StockInRepository;
using StockDemo.API.Repositories.StockOutRepository;
using StockDemo.API.Repositories.StockRepository;
using System.Linq;
using System.Threading.Tasks;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository stockRepository;
        private readonly IStockInRepository stockInRepository;
        private readonly IStockOutRepository stockOutRepository;
        private readonly IMapper mapper;

        public StocksController(IStockRepository stockRepository, IStockInRepository stockInRepository, IStockOutRepository stockOutRepository, IMapper mapper)
        {
            this.stockRepository = stockRepository;
            this.stockInRepository = stockInRepository;
            this.stockOutRepository = stockOutRepository;
            this.mapper = mapper;
        }

        // GET: api/stocks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await stockRepository.GetAllStocksWithDetailsAsync();
            var stockDtos = mapper.Map<List<StockDto>>(stocks);

            return Ok(ApiResponse<List<StockDto>>.SuccessResult(stockDtos, "Lấy danh sách tồn kho thành công"));
        }

        // GET: api/stocks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await stockRepository.GetStockWithDetailsAsync(id);

            if (stock == null)
            {
                return NotFound(ApiResponse<StockDto>.ErrorResult("Không tìm thấy tồn kho"));
            }

            var stockDto = mapper.Map<StockDto>(stock);
            return Ok(ApiResponse<StockDto>.SuccessResult(stockDto, "Lấy thông tin tồn kho thành công"));
        }

        // GET: api/stocks/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId)
        {
            var stocks = await stockRepository.GetByProductAsync(productId);
            var stockDtos = mapper.Map<List<StockDto>>(stocks);

            return Ok(ApiResponse<List<StockDto>>.SuccessResult(stockDtos, "Lấy tồn kho theo sản phẩm thành công"));
        }

        // GET: api/stocks/location/{locationId}
        [HttpGet("location/{locationId}")]
        public async Task<IActionResult> GetByLocation([FromRoute] int locationId)
        {
            var stocks = await stockRepository.GetByLocationAsync(locationId);
            var stockDtos = mapper.Map<List<StockDto>>(stocks);

            return Ok(ApiResponse<List<StockDto>>.SuccessResult(stockDtos, "Lấy tồn kho theo vị trí thành công"));
        }

        // GET: api/stocks/qrcode/{qrCode}
        [HttpGet("qrcode/{qrCode}")]
        public async Task<IActionResult> GetByQRCode([FromRoute] string qrCode)
        {
            var stocks = await stockRepository.GetByQRCodeAsync(qrCode);
            var stockDto = mapper.Map<StockDto>(stocks);

            return Ok(ApiResponse<StockDto>.SuccessResult(stockDto, "Lấy tồn kho theo QR Code thành công"));
        }

        // POST: api/stocks
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateStockDto createStockDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ApiResponse<StockDto>.ErrorResult(
        //            "Dữ liệu không hợp lệ",
        //            ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
        //        ));
        //    }

        //    var stock = mapper.Map<Stock>(createStockDto);
        //    stock.LastUpdated = DateTime.Now;

        //    var createdStock = await stockRepository.AddAsync(stock);
        //    var stockDto = mapper.Map<StockDto>(await stockRepository.GetStockWithDetailsAsync(createdStock.StockId));

        //    return CreatedAtAction(
        //        nameof(GetById),
        //        new { id = createdStock.StockId },
        //        ApiResponse<StockDto>.SuccessResult(stockDto, "Tạo tồn kho thành công")
        //    );
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto createStockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            // Check có tồn tại trong Stock không
            var stock = await stockRepository.GetByQRCodeAsync(createStockDto.QRCode);

            var stockModel = mapper.Map<Stock>(createStockDto);

            if (stock == null)
            {
                // Không tồn tại => Insert
                var createdStock = await stockRepository.AddAsync(stockModel);
            }
            else
            {
                // Có tồn tại => Update
                var success = await stockRepository.UpdateQuantityAsync(stock.StockId, createStockDto.Quantity);

                if (!success)
                {
                    return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy tồn kho"));
                }

            }

            // Insert vào stockin
            var stockIn = mapper.Map<StockIn>(createStockDto);
            stockIn.CreatedBy = createStockDto.UserId;
            stockIn.CreatedDate = DateTime.Now;

            var createdStockIn = await stockInRepository.AddAsync(stockIn);
            var stockInDto = mapper.Map<StockInDto>(await stockInRepository.GetStockInWithDetailsAsync(createdStockIn.StockInId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdStockIn.StockInId },
                ApiResponse<StockInDto>.SuccessResult(stockInDto, "Tạo phiếu nhập thành công")
            );
        }



        // PUT: api/stocks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var stock = await stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(ApiResponse<StockDto>.ErrorResult("Không tìm thấy tồn kho"));
            }

            // Cập nhật thông tin
            if (updateStockDto.Quantity.HasValue)
                stock.Quantity = updateStockDto.Quantity.Value;

            if (!string.IsNullOrEmpty(updateStockDto.QRCode))
                stock.QRCode = updateStockDto.QRCode;

            stock.LastUpdated = DateTime.Now;

            await stockRepository.UpdateAsync(stock);


            var stockDto = mapper.Map<StockDto>(await stockRepository.GetStockWithDetailsAsync(id));

            return Ok(ApiResponse<StockDto>.SuccessResult(stockDto, "Cập nhật tồn kho thành công"));
        }

        // PUT: api/stocks/{id}/quantity
        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int id, [FromBody] UpdateQuantityDto updateQuantityDto)
        {
            // Check tồn tại
            var stock = await stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound(ApiResponse<StockDto>.ErrorResult("Không tìm thấy tồn kho"));
            }

            var success = await stockRepository.UpdateQuantityAsync(id, updateQuantityDto.Quantity);

            if (!success)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy tồn kho"));
            }


            // Insert vào Stock Out
            var stockOut = mapper.Map<StockOut>(stock);
            stockOut.Quantity = updateQuantityDto.Quantity;
            stockOut.CreatedBy = updateQuantityDto.CreatedBy;
            stockOut.CreatedDate = DateTime.Now;

            var createdStockOut = await stockOutRepository.AddAsync(stockOut);
            var stockOutDto = mapper.Map<StockOutDto>(await stockOutRepository.GetStockOutWithDetailsAsync(createdStockOut.StockOutId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdStockOut.StockOutId },
                ApiResponse<StockOutDto>.SuccessResult(stockOutDto, "Tạo phiếu xuất thành công")
            );
        }

        // DELETE: api/stocks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var success = await stockRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy tồn kho"));
            }

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa tồn kho thành công"));
        }
    }
}