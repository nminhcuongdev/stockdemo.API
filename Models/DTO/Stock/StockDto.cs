using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDemo.API.Models.DTO.Stock
{
    public class StockDto
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }

        public int LocationId { get; set; }

        public int Quantity { get; set; }

        public string QRCode { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public ProductDto Product { get; set; }

        public LocationDto Location { get; set; }
    }
}
