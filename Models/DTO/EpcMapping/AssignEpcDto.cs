using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.EpcMapping
{
    public class AssignEpcDto
    {
        [Required(ErrorMessage = "EPC là bắt buộc")]
        [MaxLength(100)]
        public string Epc { get; set; }

        [Required(ErrorMessage = "Mã QR sản phẩm là bắt buộc")]
        public string QRCode { get; set; }
    }
}
