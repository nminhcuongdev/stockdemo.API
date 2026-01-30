using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.StockOutRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockOutsController : ControllerBase
    {
        private readonly IStockOutRepository stockOutRepository;
        private readonly IMapper mapper;

        public StockOutsController(IStockOutRepository stockOutRepository, IMapper mapper)
        {
            this.stockOutRepository = stockOutRepository;
            this.mapper = mapper;
        }

        // GET: api/stockouts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stockOuts = await stockOutRepository.GetAllStockOutsWithDetailsAsync();
            var stockOutDtos = mapper.Map<List<StockOutDto>>(stockOuts);

            return Ok(ApiResponse<List<StockOutDto>>.SuccessResult(stockOutDtos, "Lấy danh sách phiếu xuất thành công"));
        }

        // GET: api/stockouts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stockOut = await stockOutRepository.GetStockOutWithDetailsAsync(id);

            if (stockOut == null)
            {
                return NotFound(ApiResponse<StockOutDto>.ErrorResult("Không tìm thấy phiếu xuất"));
            }

            var stockOutDto = mapper.Map<StockOutDto>(stockOut);
            return Ok(ApiResponse<StockOutDto>.SuccessResult(stockOutDto, "Lấy thông tin phiếu xuất thành công"));
        }

        // GET: api/stockouts/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId)
        {
            var stockOuts = await stockOutRepository.GetByProductAsync(productId);
            var stockOutDtos = mapper.Map<List<StockOutDto>>(stockOuts);

            return Ok(ApiResponse<List<StockOutDto>>.SuccessResult(stockOutDtos, "Lấy phiếu xuất theo sản phẩm thành công"));
        }

        // GET: api/stockouts/daterange?startDate=...&endDate=...
        [HttpGet("daterange")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stockOuts = await stockOutRepository.GetByDateRangeAsync(startDate, endDate);
            var stockOutDtos = mapper.Map<List<StockOutDto>>(stockOuts);

            return Ok(ApiResponse<List<StockOutDto>>.SuccessResult(stockOutDtos, "Lấy phiếu xuất theo khoảng thời gian thành công"));
        }

        // POST: api/stockouts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockOutDto createStockOutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockOutDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }


            var stockOut = mapper.Map<StockOut>(createStockOutDto);
            stockOut.CreatedDate = DateTime.Now;

            var createdStockOut = await stockOutRepository.AddAsync(stockOut);
            var stockOutDto = mapper.Map<StockOutDto>(await stockOutRepository.GetStockOutWithDetailsAsync(createdStockOut.StockOutId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdStockOut.StockOutId },
                ApiResponse<StockOutDto>.SuccessResult(stockOutDto, "Tạo phiếu xuất thành công")
            );
        }

        // PUT: api/stockouts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockOutDto updateStockOutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<StockOutDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var stockOut = await stockOutRepository.GetByIdAsync(id);

            if (stockOut == null)
            {
                return NotFound(ApiResponse<StockOutDto>.ErrorResult("Không tìm thấy phiếu xuất"));
            }

            if (updateStockOutDto.ProductId.HasValue)
                stockOut.ProductId = updateStockOutDto.ProductId.Value;

            if (updateStockOutDto.LocationId.HasValue)
                stockOut.LocationId = updateStockOutDto.LocationId.Value;

            if (updateStockOutDto.Quantity.HasValue)
                stockOut.Quantity = updateStockOutDto.Quantity.Value;

            if (!string.IsNullOrEmpty(updateStockOutDto.QRCode))
                stockOut.QRCode = updateStockOutDto.QRCode;

            if (!string.IsNullOrEmpty(updateStockOutDto.Status))

            await stockOutRepository.UpdateAsync(stockOut);
            var stockOutDto = mapper.Map<StockOutDto>(await stockOutRepository.GetStockOutWithDetailsAsync(id));

            return Ok(ApiResponse<StockOutDto>.SuccessResult(stockOutDto, "Cập nhật phiếu xuất thành công"));
        }

        // DELETE: api/stockouts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var success = await stockOutRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy phiếu xuất"));
            }

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa phiếu xuất thành công"));
        }
    }
}
