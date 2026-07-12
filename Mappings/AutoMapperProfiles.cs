using AutoMapper;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models.DTO.DeliveryOrder;
using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.Stock;
using StockDemo.API.Models.DTO.User;

namespace StockDemo.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<StockDto, Stock>().ReverseMap();
            CreateMap<Stock, CreateStockDto>().ReverseMap();
            CreateMap<StockIn, CreateStockDto>().ReverseMap();
            CreateMap<StockIn, StockInDto>().ReverseMap();

            CreateMap<StockOut, StockOutDto>().ReverseMap();
            CreateMap<Stock, StockOut>().ReverseMap();
            CreateMap<StockOutDto, StockOut>().ReverseMap();

            CreateMap<StockTransfer, StockTransferDto>().ReverseMap();

            CreateMap<StockTake, StockTakeDto>().ReverseMap();
            CreateMap<StockTakeItem, StockTakeItemDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<DeliveryOrderDto, DeliveryOrder>().ReverseMap();

            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();
            CreateMap<Location, CreateLocationDto>().ReverseMap();
        }
    }
}
