namespace StockDemo.API.Models.DTO.EpcMapping
{
    public class EpcMappingDto
    {
        public string Epc { get; set; }
        public int StockId { get; set; }
        public string QRCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LocationName { get; set; }
        public int Quantity { get; set; }
        public DateTime MappedDate { get; set; }
    }
}
