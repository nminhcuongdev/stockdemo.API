using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("StockIn")]
    public class StockIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockInId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }

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

        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [ForeignKey("CreatedBy")]
        public User User { get; set; }
    }
}
