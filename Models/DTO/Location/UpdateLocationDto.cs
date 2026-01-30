using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Location
{
    public class UpdateLocationDto
    {
        [MaxLength(50)]
        public string LocationCode { get; set; }

        [MaxLength(200)]
        public string LocationName { get; set; }

        public bool? IsActive { get; set; }
    }
}
