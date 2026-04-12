using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDemo.API.Models;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.DeliveryOrder;
using StockDemo.API.Repositories.DeliveryOderRepository;
using StockDemo.API.Repositories.ProductRepository;

namespace StockDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryOrdersController : ControllerBase
    {
        private readonly IDeliveryOrderRepository deliveryOrderRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public DeliveryOrdersController(
            IDeliveryOrderRepository deliveryOrderRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            this.deliveryOrderRepository = deliveryOrderRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        // GET: api/deliveryorders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliveryOrders = await deliveryOrderRepository.GetAllDeliveryOrdersWithDetailsAsync();
            var deliveryOrderDtos = mapper.Map<List<DeliveryOrderDto>>(deliveryOrders);

            return Ok(ApiResponse<List<DeliveryOrderDto>>.SuccessResult(deliveryOrderDtos, "Lấy danh sách đơn giao hàng thành công"));
        }

        // GET: api/deliveryorders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var deliveryOrder = await deliveryOrderRepository.GetDeliveryOrderWithDetailsAsync(id);

            if (deliveryOrder == null)
            {
                return NotFound(ApiResponse<DeliveryOrderDto>.ErrorResult("Không tìm thấy đơn giao hàng"));
            }

            var deliveryOrderDto = mapper.Map<DeliveryOrderDto>(deliveryOrder);
            return Ok(ApiResponse<DeliveryOrderDto>.SuccessResult(deliveryOrderDto, "Lấy thông tin đơn giao hàng thành công"));
        }

        // GET: api/deliveryorders/po/{poNumber}
        [HttpGet("po/{poNumber}")]
        public async Task<IActionResult> GetByPONumber([FromRoute] string poNumber)
        {
            var deliveryOrder = await deliveryOrderRepository.GetByPONumberAsync(poNumber);

            if (deliveryOrder == null)
            {
                return NotFound(ApiResponse<DeliveryOrderDto>.ErrorResult("Không tìm thấy đơn giao hàng"));
            }

            var deliveryOrderDto = mapper.Map<DeliveryOrderDto>(deliveryOrder);
            return Ok(ApiResponse<DeliveryOrderDto>.SuccessResult(deliveryOrderDto, "Lấy thông tin đơn giao hàng thành công"));
        }

        // GET: api/deliveryorders/qrcode/{qrCode}
        [HttpGet("qrcode/{qrCode}")]
        public async Task<IActionResult> GetByQRCode([FromRoute] string qrCode)
        {
            var deliveryOrder = await deliveryOrderRepository.GetByQRCodeAsync(qrCode);

            if (deliveryOrder == null)
            {
                return NotFound(ApiResponse<DeliveryOrderDto>.ErrorResult("Không tìm thấy đơn giao hàng"));
            }

            var deliveryOrderDto = mapper.Map<DeliveryOrderDto>(deliveryOrder);
            return Ok(ApiResponse<DeliveryOrderDto>.SuccessResult(deliveryOrderDto, "Lấy thông tin đơn giao hàng thành công"));
        }

        // GET: api/deliveryorders/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus([FromRoute] string status)
        {
            var deliveryOrders = await deliveryOrderRepository.GetByStatusAsync(status);
            var deliveryOrderDtos = mapper.Map<List<DeliveryOrderDto>>(deliveryOrders);

            return Ok(ApiResponse<List<DeliveryOrderDto>>.SuccessResult(deliveryOrderDtos, $"Lấy đơn giao hàng theo trạng thái {status} thành công"));
        }

        // GET: api/deliveryorders/product/{productId}
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId)
        {
            var deliveryOrders = await deliveryOrderRepository.GetByProductAsync(productId);
            var deliveryOrderDtos = mapper.Map<List<DeliveryOrderDto>>(deliveryOrders);

            return Ok(ApiResponse<List<DeliveryOrderDto>>.SuccessResult(deliveryOrderDtos, "Lấy đơn giao hàng theo sản phẩm thành công"));
        }

        // GET: api/deliveryorders/daterange?startDate=...&endDate=...
        [HttpGet("daterange")]
        public async Task<IActionResult> GetByDeliveryDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var deliveryOrders = await deliveryOrderRepository.GetByDeliveryDateRangeAsync(startDate, endDate);
            var deliveryOrderDtos = mapper.Map<List<DeliveryOrderDto>>(deliveryOrders);

            return Ok(ApiResponse<List<DeliveryOrderDto>>.SuccessResult(deliveryOrderDtos, "Lấy đơn giao hàng theo khoảng thời gian thành công"));
        }

        // POST: api/deliveryorders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryOrderDto createDeliveryOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<DeliveryOrderDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            // Kiểm tra PO Number đã tồn tại
            if (await deliveryOrderRepository.IsPONumberExistsAsync(createDeliveryOrderDto.PONumber))
            {
                return BadRequest(ApiResponse<DeliveryOrderDto>.ErrorResult("Số PO đã tồn tại"));
            }

            // Lấy thông tin product để tạo QR Code
            var product = await productRepository.GetByIdAsync(createDeliveryOrderDto.ProductId);
            if (product == null)
            {
                return BadRequest(ApiResponse<DeliveryOrderDto>.ErrorResult("Không tìm thấy sản phẩm"));
            }

            var deliveryOrder = mapper.Map<DeliveryOrder>(createDeliveryOrderDto);

            // Generate QR Code: ProductCode;DeliveryDate;PONumber
            deliveryOrder.QRCode = deliveryOrderRepository.GenerateQRCode(
                product.ProductCode,
                createDeliveryOrderDto.DeliveryDate,
                createDeliveryOrderDto.PONumber
            );

            deliveryOrder.CreatedDate = DateTime.Now;
            deliveryOrder.Status = "Pending";

            var createdDeliveryOrder = await deliveryOrderRepository.AddAsync(deliveryOrder);
            var deliveryOrderDto = mapper.Map<DeliveryOrderDto>(
                await deliveryOrderRepository.GetDeliveryOrderWithDetailsAsync(createdDeliveryOrder.DeliveryOrderId)
            );

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDeliveryOrder.DeliveryOrderId },
                ApiResponse<DeliveryOrderDto>.SuccessResult(deliveryOrderDto, "Tạo đơn giao hàng thành công")
            );
        }

        // PUT: api/deliveryorders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDeliveryOrderDto updateDeliveryOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<DeliveryOrderDto>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                ));
            }

            var deliveryOrder = await deliveryOrderRepository.GetDeliveryOrderWithDetailsAsync(id);

            if (deliveryOrder == null)
            {
                return NotFound(ApiResponse<DeliveryOrderDto>.ErrorResult("Không tìm thấy đơn giao hàng"));
            }

            // Kiểm tra PO Number mới có trùng không
            if (!string.IsNullOrEmpty(updateDeliveryOrderDto.PONumber) &&
                await deliveryOrderRepository.IsPONumberExistsAsync(updateDeliveryOrderDto.PONumber, id))
            {
                return BadRequest(ApiResponse<DeliveryOrderDto>.ErrorResult("Số PO đã tồn tại"));
            }

            bool needRegenerateQRCode = false;

            // Cập nhật thông tin
            if (updateDeliveryOrderDto.ProductId.HasValue && updateDeliveryOrderDto.ProductId.Value != deliveryOrder.ProductId)
            {
                deliveryOrder.ProductId = updateDeliveryOrderDto.ProductId.Value;
                needRegenerateQRCode = true;
            }

            if (!string.IsNullOrEmpty(updateDeliveryOrderDto.PONumber) && updateDeliveryOrderDto.PONumber != deliveryOrder.PONumber)
            {
                deliveryOrder.PONumber = updateDeliveryOrderDto.PONumber;
                needRegenerateQRCode = true;
            }

            if (updateDeliveryOrderDto.DeliveryDate.HasValue && updateDeliveryOrderDto.DeliveryDate.Value != deliveryOrder.DeliveryDate)
            {
                deliveryOrder.DeliveryDate = updateDeliveryOrderDto.DeliveryDate.Value;
                needRegenerateQRCode = true;
            }

            if (updateDeliveryOrderDto.Quantity.HasValue)
                deliveryOrder.Quantity = updateDeliveryOrderDto.Quantity.Value;

            if (!string.IsNullOrEmpty(updateDeliveryOrderDto.Status))
                deliveryOrder.Status = updateDeliveryOrderDto.Status;

            // Regenerate QR Code nếu cần
            if (needRegenerateQRCode)
            {
                var product = await productRepository.GetByIdAsync(deliveryOrder.ProductId);
                if (product != null)
                {
                    deliveryOrder.QRCode = deliveryOrderRepository.GenerateQRCode(
                        product.ProductCode,
                        deliveryOrder.DeliveryDate,
                        deliveryOrder.PONumber
                    );
                }
            }

            deliveryOrder.UpdatedDate = DateTime.Now;

            await deliveryOrderRepository.UpdateAsync(deliveryOrder);
            var deliveryOrderDto = mapper.Map<DeliveryOrderDto>(
                await deliveryOrderRepository.GetDeliveryOrderWithDetailsAsync(id)
            );

            return Ok(ApiResponse<DeliveryOrderDto>.SuccessResult(deliveryOrderDto, "Cập nhật đơn giao hàng thành công"));
        }

        // DELETE: api/deliveryorders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var success = await deliveryOrderRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(ApiResponse<object>.ErrorResult("Không tìm thấy đơn giao hàng"));
            }

            return Ok(ApiResponse<object>.SuccessResult(null, "Xóa đơn giao hàng thành công"));
        }

        // POST: api/deliveryorders/bulk-import
        [HttpPost("bulk-import")]
        public async Task<IActionResult> BulkImport([FromBody] List<BulkCreateDeliveryOrderDto> bulkOrders)
        {
            if (bulkOrders == null || !bulkOrders.Any())
            {
                return BadRequest(ApiResponse<List<DeliveryOrderDto>>.ErrorResult("Danh sách không được rỗng"));
            }

            // Validate ModelState cho từng item
            var validationErrors = new List<string>();
            for (int i = 0; i < bulkOrders.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(bulkOrders[i].ProductCode))
                    validationErrors.Add($"Dòng {i + 1}: ProductCode không được để trống");

                if (string.IsNullOrWhiteSpace(bulkOrders[i].PONumber))
                    validationErrors.Add($"Dòng {i + 1}: PONumber không được để trống");

                if (bulkOrders[i].Quantity <= 0)
                    validationErrors.Add($"Dòng {i + 1}: Số lượng phải lớn hơn 0");
            }

            if (validationErrors.Any())
            {
                return BadRequest(ApiResponse<List<DeliveryOrderDto>>.ErrorResult(
                    "Dữ liệu không hợp lệ",
                    validationErrors
                ));
            }

            // Lấy tất cả ProductCode để validate và tìm Product
            var productCodes = bulkOrders.Select(x => x.ProductCode).Distinct().ToList();
            var productsList = await productRepository.GetByProductCodesAsync(productCodes);
            var products = productsList.ToDictionary(p => p.ProductCode, p => p);

            // Kiểm tra ProductCode không tồn tại
            var notFoundProductCodes = productCodes.Except(products.Keys).ToList();
            if (notFoundProductCodes.Any())
            {
                return BadRequest(ApiResponse<List<DeliveryOrderDto>>.ErrorResult(
                    $"Không tìm thấy sản phẩm với mã: {string.Join(", ", notFoundProductCodes)}"
                ));
            }

            // Kiểm tra trùng lặp trong file import (cả 3 trường)
            var duplicatesInFile = bulkOrders
                .GroupBy(x => new { x.ProductCode, x.PONumber, DeliveryDate = x.DeliveryDate.Date })
                .Where(g => g.Count() > 1)
                .Select(g => $"ProductCode: {g.Key.ProductCode}, PO: {g.Key.PONumber}, Ngày: {g.Key.DeliveryDate:yyyy-MM-dd}")
                .ToList();

            if (duplicatesInFile.Any())
            {
                return BadRequest(ApiResponse<List<DeliveryOrderDto>>.ErrorResult(
                    $"Dữ liệu bị trùng lặp trong file: {string.Join("; ", duplicatesInFile)}"
                ));
            }

            // Kiểm tra trùng lặp với database (cả 3 trường)
            var ordersToCheck = bulkOrders.Select(x => (
                ProductId: products[x.ProductCode].ProductId,
                PONumber: x.PONumber,
                DeliveryDate: x.DeliveryDate
            )).ToList();

            var duplicatesInDb = await deliveryOrderRepository.GetDuplicateOrdersAsync(ordersToCheck);

            if (duplicatesInDb.Any())
            {
                var duplicateInfo = duplicatesInDb.Select(d =>
                    $"ProductCode: {d.Product.ProductCode}, PO: {d.PONumber}, Ngày: {d.DeliveryDate:yyyy-MM-dd}"
                ).ToList();

                return BadRequest(ApiResponse<List<DeliveryOrderDto>>.ErrorResult(
                    $"Dữ liệu đã tồn tại trong hệ thống: {string.Join("; ", duplicateInfo)}"
                ));
            }

            // Tạo danh sách DeliveryOrder
            var deliveryOrdersToAdd = new List<DeliveryOrder>();

            foreach (var orderDto in bulkOrders)
            {
                var product = products[orderDto.ProductCode];
                var deliveryOrder = new DeliveryOrder
                {
                    ProductId = product.ProductId,
                    PONumber = orderDto.PONumber,
                    DeliveryDate = orderDto.DeliveryDate,
                    Quantity = orderDto.Quantity,
                    QRCode = deliveryOrderRepository.GenerateQRCode(
                        product.ProductCode,
                        orderDto.DeliveryDate,
                        orderDto.PONumber
                    ),
                    CreatedDate = DateTime.Now,
                    Status = "Pending"
                };

                deliveryOrdersToAdd.Add(deliveryOrder);
            }

            // Lưu tất cả bản ghi vào database
            try
            {
                var createdOrders = await deliveryOrderRepository.AddRangeAsync(deliveryOrdersToAdd);

                // Lấy lại thông tin chi tiết của các đơn đã tạo
                var deliveryOrderDtos = new List<DeliveryOrderDto>();
                foreach (var order in createdOrders)
                {
                    var detailedOrder = await deliveryOrderRepository.GetDeliveryOrderWithDetailsAsync(order.DeliveryOrderId);
                    deliveryOrderDtos.Add(mapper.Map<DeliveryOrderDto>(detailedOrder));
                }

                return Ok(ApiResponse<List<DeliveryOrderDto>>.SuccessResult(
                    deliveryOrderDtos,
                    $"Import thành công {deliveryOrderDtos.Count} đơn giao hàng"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<DeliveryOrderDto>>.ErrorResult(
                    $"Lỗi khi lưu dữ liệu: {ex.Message}"
                ));
            }
        }
    }
}
