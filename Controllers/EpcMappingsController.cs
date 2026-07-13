using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.EpcMapping;
using StockDemo.API.Repositories.EpcMappingRepository;
using StockDemo.API.Repositories.StockRepository;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EpcMappingsController : ControllerBase
    {
        private readonly IEpcMappingRepository epcMappingRepository;
        private readonly IStockRepository stockRepository;

        public EpcMappingsController(IEpcMappingRepository epcMappingRepository, IStockRepository stockRepository)
        {
            this.epcMappingRepository = epcMappingRepository;
            this.stockRepository = stockRepository;
        }

        // GET: api/epcmappings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mappings = await epcMappingRepository.GetAllAsync();
            var dtos = mappings.Select(MapToDto).ToList();
            return Ok(ApiResponse<List<EpcMappingDto>>.SuccessResult(dtos, "Lấy danh sách gán EPC thành công"));
        }

        // GET: api/epcmappings/{epc}
        [HttpGet("{epc}")]
        public async Task<IActionResult> GetByEpc(string epc)
        {
            var mapping = await epcMappingRepository.GetByEpcAsync(epc);
            if (mapping == null)
            {
                return NotFound(ApiResponse<EpcMappingDto>.ErrorResult("EPC chưa được gán sản phẩm"));
            }

            return Ok(ApiResponse<EpcMappingDto>.SuccessResult(MapToDto(mapping), "Lấy thông tin gán EPC thành công"));
        }

        // POST: api/epcmappings
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignEpcDto assignEpcDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<EpcMappingDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var stock = await stockRepository.GetByQRCodeAsync(assignEpcDto.QRCode);
            if (stock == null)
            {
                return NotFound(ApiResponse<EpcMappingDto>.ErrorResult("Không tìm thấy tồn kho với mã QR này"));
            }

            var mapping = await epcMappingRepository.AssignAsync(assignEpcDto.Epc, stock.StockId);
            return Ok(ApiResponse<EpcMappingDto>.SuccessResult(MapToDto(mapping), "Gán EPC thành công"));
        }

        // DELETE: api/epcmappings/{epc}
        [HttpDelete("{epc}")]
        public async Task<IActionResult> Delete(string epc)
        {
            var deleted = await epcMappingRepository.DeleteAsync(epc);
            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.ErrorResult("Không tìm thấy EPC đã gán"));
            }

            return Ok(ApiResponse<bool>.SuccessResult(true, "Xóa gán EPC thành công"));
        }

        private static EpcMappingDto MapToDto(EpcMapping mapping) => new EpcMappingDto
        {
            Epc = mapping.Epc,
            StockId = mapping.StockId,
            QRCode = mapping.Stock?.QRCode,
            ProductCode = mapping.Stock?.Product?.ProductCode,
            ProductName = mapping.Stock?.Product?.ProductName,
            LocationName = mapping.Stock?.Location?.LocationName,
            Quantity = mapping.Stock?.Quantity ?? 0,
            MappedDate = mapping.MappedDate
        };
    }
}
