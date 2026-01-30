using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string LocationName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<StockIn> StockIns { get; set; }
        public ICollection<StockOut> StockOuts { get; set; }
    }
}
