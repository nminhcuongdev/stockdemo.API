using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDemo.API.Models.Domain
{
    /// <summary>Pairs a physical RFID tag's EPC with a Stock, so a scanned EPC can be
    /// resolved to product info server-side (shared across devices) instead of each
    /// device keeping its own local pairing.</summary>
    [Table("EpcMappings")]
    public class EpcMapping
    {
        [Key]
        [MaxLength(100)]
        public string Epc { get; set; }

        [Required]
        public int StockId { get; set; }

        [ForeignKey("StockId")]
        public Stock Stock { get; set; }

        public DateTime MappedDate { get; set; } = DateTime.Now;
    }
}
