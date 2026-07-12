using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("StockTakes")]
    public class StockTake
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockTakeId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "InProgress"; // InProgress, Completed, Cancelled

        [MaxLength(500)]
        public string? Note { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        // Foreign keys
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [ForeignKey("CreatedBy")]
        public User User { get; set; }

        public ICollection<StockTakeItem> Items { get; set; }
    }

    [Table("StockTakeItems")]
    public class StockTakeItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockTakeItemId { get; set; }

        [Required]
        public int StockTakeId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SystemQuantity { get; set; }

        [Required]
        public int CountedQuantity { get; set; }

        [Required]
        public int Variance { get; set; } // CountedQuantity - SystemQuantity

        // Foreign keys
        [ForeignKey("StockTakeId")]
        public StockTake StockTake { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
