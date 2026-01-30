using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
using StockDemo.API.Repositories.ProductRepository;
using StockDemo.API.Models.DTO.Product;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productRepository.GetAllAsync();
            var productDtos = mapper.Map<List<ProductDto>>(products);

            return Ok(ApiResponse<List<ProductDto>>.SuccessResult(productDtos, "Lấy danh sách sản phẩm thành công"));
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<ProductDto>.ErrorResult("Không tìm thấy sản phẩm"));
            }

            var productDto = mapper.Map<ProductDto>(product);
            return Ok(ApiResponse<ProductDto>.SuccessResult(productDto, "Lấy thông tin sản phẩm thành công"));
        }

        // GET: api/products/code/{code}
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode([FromRoute] string code)
        {
            var product = await productRepository.GetByCodeAsync(code);

            if (product == null)
            {
                return NotFound(ApiResponse<ProductDto>.ErrorResult("Không tìm thấy sản phẩm"));
            }

            var productDto = mapper.Map<ProductDto>(product);
            return Ok(ApiResponse<ProductDto>.SuccessResult(productDto, "Lấy thông tin sản phẩm thành công"));
        }

        // GET: api/products/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await productRepository.GetActiveProductsAsync();
            var productDtos = mapper.Map<List<ProductDto>>(products);

            return Ok(ApiResponse<List<ProductDto>>.SuccessResult(productDtos, "Lấy danh sách sản phẩm active thành công"));
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ProductDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            // Kiểm tra mã sản phẩm đã tồn tại
            if (await productRepository.IsProductCodeExistsAsync(createProductDto.ProductCode))
            {
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("Mã sản phẩm đã tồn tại"));
            }

            var product = mapper.Map<Product>(createProductDto);
            product.CreatedDate = DateTime.Now;
            product.IsActive = true;

            var createdProduct = await productRepository.AddAsync(product);
            var productDto = mapper.Map<ProductDto>(createdProduct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProduct.ProductId },
                ApiResponse<ProductDto>.SuccessResult(productDto, "Tạo sản phẩm thành công")
            );
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ProductDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<ProductDto>.ErrorResult("Không tìm thấy sản phẩm"));
            }

            // Kiểm tra mã sản phẩm mới có trùng không
            if (!string.IsNullOrEmpty(updateProductDto.ProductCode) &&
                await productRepository.IsProductCodeExistsAsync(updateProductDto.ProductCode, id))
            {
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("Mã sản phẩm đã tồn tại"));
            }

            // Cập nhật thông tin
            if (!string.IsNullOrEmpty(updateProductDto.ProductCode))
                product.ProductCode = updateProductDto.ProductCode;

            if (!string.IsNullOrEmpty(updateProductDto.ProductName))
                product.ProductName = updateProductDto.ProductName;

            if (updateProductDto.Description != null)
                product.Description = updateProductDto.Description;

            if (!string.IsNullOrEmpty(updateProductDto.Unit))
                product.Unit = updateProductDto.Unit;

            if (updateProductDto.IsActive.HasValue)
                product.IsActive = updateProductDto.IsActive.Value;

            product.UpdatedDate = DateTime.Now;

            await productRepository.UpdateAsync(product);
            var productDto = mapper.Map<ProductDto>(product);

            return Ok(ApiResponse<ProductDto>.SuccessResult(productDto, "Cập nhật sản phẩm thành công"));
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy sản phẩm"));
            }

            // Soft delete
            product.IsActive = false;
            product.UpdatedDate = DateTime.Now;
            await productRepository.DeleteAsync(id);

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa sản phẩm thành công"));
        }
    }
}
