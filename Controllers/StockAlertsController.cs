using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Repositories.ProductRepository;
using StockDemo.API.Repositories.StockRepository;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockAlertsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IStockRepository stockRepository;
        private readonly IMapper mapper;

        public StockAlertsController(
            IProductRepository productRepository,
            IStockRepository stockRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.stockRepository = stockRepository;
            this.mapper = mapper;
        }

        // GET: api/stockalerts/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var products = await productRepository.GetAllAsync();
            var stocks = await stockRepository.GetAllAsync();

            // Total on-hand quantity per product (across all locations).
            var onHandByProduct = stocks
                .GroupBy(s => s.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity));

            var lowStock = products
                .Where(p => p.IsActive && p.MinQuantity > 0)
                .Select(p =>
                {
                    var current = onHandByProduct.TryGetValue(p.ProductId, out var qty) ? qty : 0;
                    return new LowStockItemDto
                    {
                        ProductId = p.ProductId,
                        Product = mapper.Map<ProductDto>(p),
                        CurrentQuantity = current,
                        MinQuantity = p.MinQuantity,
                        MaxQuantity = p.MaxQuantity,
                        Shortage = p.MinQuantity - current
                    };
                })
                .Where(item => item.CurrentQuantity < item.MinQuantity)
                .OrderByDescending(item => item.Shortage)
                .ToList();

            return Ok(ApiResponse<List<LowStockItemDto>>.SuccessResult(lowStock, "Lấy danh sách cảnh báo tồn thấp thành công"));
        }
    }
}
