using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(200)]
        public string QRCode { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Foreign keys
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
}
