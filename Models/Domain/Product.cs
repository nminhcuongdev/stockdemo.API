using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; }

        public bool IsActive { get; set; } = true;

        // Reorder level: alert when total on-hand quantity drops below this. 0 = no alert.
        public int MinQuantity { get; set; } = 0;

        // Optional maximum stock level (over-stock reference).
        public int? MaxQuantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<StockIn> StockIns { get; set; }
        public ICollection<StockOut> StockOuts { get; set; }
        public ICollection<DeliveryOrder> DeliveryOrders { get; set; }
    }
}
