using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Device;
using StockDemo.API.Repositories.DeviceTokenRepository;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceTokenRepository deviceTokenRepository;

        public DevicesController(IDeviceTokenRepository deviceTokenRepository)
        {
            this.deviceTokenRepository = deviceTokenRepository;
        }

        // POST: api/devices/register-token
        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterDeviceTokenDto dto)
        {
            var existing = await deviceTokenRepository.GetByTokenAsync(dto.Token);
            if (existing != null)
            {
                existing.UserId = dto.UserId ?? existing.UserId;
                existing.Platform = dto.Platform ?? existing.Platform;
                existing.Locale = dto.Locale ?? existing.Locale;
                existing.LastSeen = DateTime.Now;
                await deviceTokenRepository.UpdateAsync(existing);
            }
            else
            {
                await deviceTokenRepository.AddAsync(new DeviceToken
                {
                    Token = dto.Token,
                    UserId = dto.UserId,
                    Platform = dto.Platform ?? "android",
                    Locale = dto.Locale,
                    CreatedDate = DateTime.Now,
                    LastSeen = DateTime.Now
                });
            }

            return Ok(ApiResponse<object>.SuccessResult(null, "Đăng ký thiết bị nhận thông báo thành công"));
        }
    }
}
