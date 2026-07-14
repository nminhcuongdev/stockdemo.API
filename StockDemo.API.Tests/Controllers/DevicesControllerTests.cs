using Microsoft.AspNetCore.Mvc;
using Moq;
using StockDemo.API.Controllers;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Device;
using StockDemo.API.Repositories.DeviceTokenRepository;
using Xunit;

namespace StockDemo.API.Tests.Controllers
{
    public class DevicesControllerTests
    {
        private readonly Mock<IDeviceTokenRepository> repo = new();

        private DevicesController CreateController() => new(repo.Object);

        [Fact]
        public async Task RegisterToken_creates_a_new_token_when_none_exists()
        {
            repo.Setup(r => r.GetByTokenAsync("new-token")).ReturnsAsync((DeviceToken?)null);
            DeviceToken? added = null;
            repo.Setup(r => r.AddAsync(It.IsAny<DeviceToken>()))
                .Callback<DeviceToken>(d => added = d)
                .ReturnsAsync((DeviceToken d) => d);

            var dto = new RegisterDeviceTokenDto { Token = "new-token", UserId = 4, Platform = "android", Locale = "en" };
            var result = await CreateController().RegisterToken(dto);

            Assert.IsType<OkObjectResult>(result);
            repo.Verify(r => r.AddAsync(It.IsAny<DeviceToken>()), Times.Once);
            repo.Verify(r => r.UpdateAsync(It.IsAny<DeviceToken>()), Times.Never);
            Assert.NotNull(added);
            Assert.Equal("new-token", added!.Token);
            Assert.Equal(4, added.UserId);
            Assert.Equal("en", added.Locale);
        }

        [Fact]
        public async Task RegisterToken_updates_existing_token_and_locale()
        {
            var existing = new DeviceToken { DeviceTokenId = 1, Token = "t", UserId = 1, Platform = "android", Locale = "vi" };
            repo.Setup(r => r.GetByTokenAsync("t")).ReturnsAsync(existing);
            repo.Setup(r => r.UpdateAsync(It.IsAny<DeviceToken>())).ReturnsAsync((DeviceToken d) => d);

            var dto = new RegisterDeviceTokenDto { Token = "t", UserId = 9, Platform = "android", Locale = "en" };
            var result = await CreateController().RegisterToken(dto);

            Assert.IsType<OkObjectResult>(result);
            repo.Verify(r => r.UpdateAsync(It.IsAny<DeviceToken>()), Times.Once);
            repo.Verify(r => r.AddAsync(It.IsAny<DeviceToken>()), Times.Never);
            Assert.Equal(9, existing.UserId);
            Assert.Equal("en", existing.Locale); // locale switched from vi to en
        }

        [Fact]
        public async Task RegisterToken_keeps_existing_locale_when_dto_omits_it()
        {
            var existing = new DeviceToken { DeviceTokenId = 1, Token = "t", UserId = 1, Locale = "vi" };
            repo.Setup(r => r.GetByTokenAsync("t")).ReturnsAsync(existing);
            repo.Setup(r => r.UpdateAsync(It.IsAny<DeviceToken>())).ReturnsAsync((DeviceToken d) => d);

            var dto = new RegisterDeviceTokenDto { Token = "t", UserId = 1, Locale = null };
            await CreateController().RegisterToken(dto);

            Assert.Equal("vi", existing.Locale);
        }

        [Fact]
        public async Task RegisterToken_returns_bad_request_on_invalid_model()
        {
            var controller = CreateController();
            controller.ModelState.AddModelError("Token", "Token là bắt buộc");

            var result = await controller.RegisterToken(new RegisterDeviceTokenDto { Token = "" });

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(payload.Success);
            repo.Verify(r => r.AddAsync(It.IsAny<DeviceToken>()), Times.Never);
            repo.Verify(r => r.UpdateAsync(It.IsAny<DeviceToken>()), Times.Never);
        }
    }
}
