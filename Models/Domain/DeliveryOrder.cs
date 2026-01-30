using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("DeliveryOrders")]
    public class DeliveryOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryOrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PONumber { get; set; }

        public DateTime DeliveryDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(200)]
        public string QRCode { get; set; } // Format: ProductCode;DeliveryDate;PONumber

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Pending, InTransit, Delivered, Cancelled

        // Foreign key
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
