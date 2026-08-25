using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Repositories.LocationRepository;
using System.Linq;
using System.Threading.Tasks;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;

        public LocationsController(ILocationRepository locationRepository, IMapper mapper)
        {
            this.locationRepository = locationRepository;
            this.mapper = mapper;
        }

        // GET: api/locations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await locationRepository.GetAllAsync();
            var locationDtos = mapper.Map<List<LocationDto>>(locations);

            return Ok(ApiResponse<List<LocationDto>>.SuccessResult(locationDtos, "Lấy danh sách vị trí thành công"));
        }

        // GET: api/locations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var location = await locationRepository.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(ApiResponse<LocationDto>.ErrorResult("Không tìm thấy vị trí"));
            }

            var locationDto = mapper.Map<LocationDto>(location);
            return Ok(ApiResponse<LocationDto>.SuccessResult(locationDto, "Lấy thông tin vị trí thành công"));
        }

        // GET: api/locations/code/{code}
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode([FromRoute] string code)
        {
            var location = await locationRepository.GetByCodeAsync(code);

            if (location == null)
            {
                return NotFound(ApiResponse<LocationDto>.ErrorResult("Không tìm thấy vị trí"));
            }

            var locationDto = mapper.Map<LocationDto>(location);
            return Ok(ApiResponse<LocationDto>.SuccessResult(locationDto, "Lấy thông tin vị trí thành công"));
        }

        // GET: api/locations/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveLocations()
        {
            var locations = await locationRepository.GetActiveLocationsAsync();
            var locationDtos = mapper.Map<List<LocationDto>>(locations);

            return Ok(ApiResponse<List<LocationDto>>.SuccessResult(locationDtos, "Lấy danh sách vị trí active thành công"));
        }

        // POST: api/locations
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationDto createLocationDto)
        {
            // Kiểm tra mã vị trí đã tồn tại
            if (await locationRepository.IsLocationCodeExistsAsync(createLocationDto.LocationCode))
            {
                return BadRequest(ApiResponse<LocationDto>.ErrorResult("Mã vị trí đã tồn tại"));
            }

            var location = mapper.Map<Location>(createLocationDto);
            location.CreatedDate = DateTime.Now;
            location.IsActive = true;

            var createdLocation = await locationRepository.AddAsync(location);
            var locationDto = mapper.Map<LocationDto>(createdLocation);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdLocation.LocationId },
                ApiResponse<LocationDto>.SuccessResult(locationDto, "Tạo vị trí thành công")
            );
        }

        // PUT: api/locations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLocationDto updateLocationDto)
        {
            var location = await locationRepository.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(ApiResponse<LocationDto>.ErrorResult("Không tìm thấy vị trí"));
            }

            // Kiểm tra mã vị trí mới có trùng không
            if (!string.IsNullOrEmpty(updateLocationDto.LocationCode) &&
                await locationRepository.IsLocationCodeExistsAsync(updateLocationDto.LocationCode, id))
            {
                return BadRequest(ApiResponse<LocationDto>.ErrorResult("Mã vị trí đã tồn tại"));
            }

            // Cập nhật thông tin
            if (!string.IsNullOrEmpty(updateLocationDto.LocationCode))
                location.LocationCode = updateLocationDto.LocationCode;

            if (!string.IsNullOrEmpty(updateLocationDto.LocationName))
                location.LocationName = updateLocationDto.LocationName;

            if (updateLocationDto.IsActive.HasValue)
                location.IsActive = updateLocationDto.IsActive.Value;

            location.UpdatedDate = DateTime.Now;

            await locationRepository.UpdateAsync(location);
            var locationDto = mapper.Map<LocationDto>(location);

            return Ok(ApiResponse<LocationDto>.SuccessResult(locationDto, "Cập nhật vị trí thành công"));
        }

        // DELETE: api/locations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var location = await locationRepository.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy vị trí"));
            }

            // Soft delete
            location.IsActive = false;
            location.UpdatedDate = DateTime.Now;
            await locationRepository.DeleteAsync(id);

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa vị trí thành công"));
        }
    }
}
