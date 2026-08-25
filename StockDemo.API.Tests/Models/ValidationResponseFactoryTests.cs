using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using Xunit;

namespace StockDemo.API.Tests.Models
{
    /// <summary>
    /// Validation failures are handled by the MVC pipeline rather than by each action, so they are
    /// asserted here against the factory that shapes that response — a controller unit test would
    /// bypass the pipeline and never see it.
    /// </summary>
    public class ValidationResponseFactoryTests
    {
        [Fact]
        public void Create_returns_bad_request_wrapped_in_the_ApiResponse_envelope()
        {
            var context = new ActionContext();
            context.ModelState.AddModelError("Token", "Token là bắt buộc");

            var result = ValidationResponseFactory.Create(context);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(payload.Success);
            Assert.Equal("Dữ liệu không hợp lệ", payload.Message);
            Assert.Null(payload.Data);
            Assert.Equal(new[] { "Token là bắt buộc" }, payload.Errors);
        }

        [Fact]
        public void Create_flattens_every_field_error_into_the_errors_array()
        {
            // The Android client parses `errors` as a flat string array, unlike the RFC 7807
            // default where it is an object keyed by field name.
            var context = new ActionContext();
            context.ModelState.AddModelError("Username", "Tên đăng nhập là bắt buộc");
            context.ModelState.AddModelError("Password", "Mật khẩu là bắt buộc");
            context.ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 6 ký tự");

            var result = ValidationResponseFactory.Create(context);

            var payload = Assert.IsType<ApiResponse<object>>(
                Assert.IsType<BadRequestObjectResult>(result).Value);
            Assert.Equal(3, payload.Errors.Count);
            Assert.Contains("Tên đăng nhập là bắt buộc", payload.Errors);
            Assert.Contains("Mật khẩu phải có ít nhất 6 ký tự", payload.Errors);
        }

        [Fact]
        public void Create_returns_an_empty_errors_array_when_nothing_is_invalid()
        {
            var result = ValidationResponseFactory.Create(new ActionContext());

            var payload = Assert.IsType<ApiResponse<object>>(
                Assert.IsType<BadRequestObjectResult>(result).Value);
            Assert.Empty(payload.Errors);
        }
    }
}
