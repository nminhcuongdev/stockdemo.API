using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("StockTransfers")]
    public class StockTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockTransferId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int FromLocationId { get; set; }

        [Required]
        public int ToLocationId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(200)]
        public string QRCode { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign keys
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("FromLocationId")]
        public Location FromLocation { get; set; }

        [ForeignKey("ToLocationId")]
        public Location ToLocation { get; set; }

        [ForeignKey("CreatedBy")]
        public User User { get; set; }
    }
}
