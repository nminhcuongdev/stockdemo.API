using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.Report;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly StockDemoDbContext dbContext;
        private readonly IMapper mapper;

        public ReportsController(StockDemoDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // GET: api/reports/stock-movement?from=2026-06-01&to=2026-06-30&locationId=1
        [HttpGet("stock-movement")]
        public async Task<IActionResult> StockMovement(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? locationId)
        {
            var fromDate = (from ?? DateTime.Now.AddDays(-30)).Date;
            // Inclusive of the whole "to" day.
            var toExclusive = (to ?? DateTime.Now).Date.AddDays(1);

            if (toExclusive <= fromDate)
            {
                return BadRequest(ApiResponse<StockMovementReportDto>.ErrorResult("Khoảng thời gian không hợp lệ"));
            }

            var insQuery = dbContext.StockIns
                .Where(x => x.CreatedDate >= fromDate && x.CreatedDate < toExclusive);
            var outsQuery = dbContext.StockOuts
                .Where(x => x.CreatedDate >= fromDate && x.CreatedDate < toExclusive);
            var stockQuery = dbContext.Stocks.AsQueryable();

            if (locationId.HasValue)
            {
                insQuery = insQuery.Where(x => x.LocationId == locationId.Value);
                outsQuery = outsQuery.Where(x => x.LocationId == locationId.Value);
                stockQuery = stockQuery.Where(x => x.LocationId == locationId.Value);
            }

            var insByProduct = (await insQuery
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Total = g.Sum(x => x.Quantity) })
                .ToListAsync())
                .ToDictionary(x => x.ProductId, x => x.Total);

            var outsByProduct = (await outsQuery
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Total = g.Sum(x => x.Quantity) })
                .ToListAsync())
                .ToDictionary(x => x.ProductId, x => x.Total);

            var stockByProduct = (await stockQuery
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Total = g.Sum(x => x.Quantity) })
                .ToListAsync())
                .ToDictionary(x => x.ProductId, x => x.Total);

            var products = await dbContext.Products.Where(p => p.IsActive).ToListAsync();

            var items = products
                .Select(p =>
                {
                    insByProduct.TryGetValue(p.ProductId, out var totalIn);
                    outsByProduct.TryGetValue(p.ProductId, out var totalOut);
                    stockByProduct.TryGetValue(p.ProductId, out var currentStock);
                    return new StockMovementReportItemDto
                    {
                        ProductId = p.ProductId,
                        Product = mapper.Map<ProductDto>(p),
                        TotalIn = totalIn,
                        TotalOut = totalOut,
                        CurrentStock = currentStock
                    };
                })
                // Only include products that had activity or still hold stock.
                .Where(i => i.TotalIn > 0 || i.TotalOut > 0 || i.CurrentStock > 0)
                .OrderByDescending(i => i.TotalIn + i.TotalOut)
                .ThenBy(i => i.Product.ProductName)
                .ToList();

            var report = new StockMovementReportDto
            {
                From = fromDate,
                To = toExclusive.AddDays(-1),
                TotalIn = items.Sum(i => i.TotalIn),
                TotalOut = items.Sum(i => i.TotalOut),
                TotalStock = items.Sum(i => i.CurrentStock),
                Items = items
            };

            return Ok(ApiResponse<StockMovementReportDto>.SuccessResult(report, "Lấy báo cáo nhập-xuất-tồn thành công"));
        }
    }
}
