using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.Domain
{
    [Table("DeviceTokens")]
    public class DeviceToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceTokenId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Token { get; set; }

        public int? UserId { get; set; }

        [MaxLength(20)]
        public string? Platform { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime LastSeen { get; set; } = DateTime.Now;
    }
}
