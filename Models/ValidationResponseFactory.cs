using Microsoft.AspNetCore.Mvc;

namespace StockDemo.API.Models
{
    /// <summary>
    /// Shapes the automatic 400 that <c>[ApiController]</c> produces for an invalid model into the
    /// same <see cref="ApiResponse{T}"/> envelope every other endpoint returns, so clients only
    /// ever parse one response format.
    /// </summary>
    /// <remarks>
    /// This runs inside the MVC filter pipeline (<c>ModelStateInvalidFilter</c>), before the action
    /// body executes. Unit tests that invoke an action method directly bypass that pipeline, so
    /// validation behaviour must be asserted here — or through an integration test — rather than
    /// by seeding <c>ModelState</c> on a hand-constructed controller.
    /// </remarks>
    public static class ValidationResponseFactory
    {
        public static IActionResult Create(ActionContext context)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                .ToList();

            return new BadRequestObjectResult(
                ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ", errors));
        }
    }
}
