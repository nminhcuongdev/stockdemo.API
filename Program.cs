using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Mappings;
using StockDemo.API.Repositories.DeliveryOderRepository;
using StockDemo.API.Repositories.LocationRepository;
using StockDemo.API.Repositories.ProductRepository;
using StockDemo.API.Repositories.StockInRepository;
using StockDemo.API.Repositories.StockOutRepository;
using StockDemo.API.Repositories.StockRepository;
using StockDemo.API.Repositories.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StockDemoDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("StockDemoConnectionString")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockInRepository, StockInRepository>();
builder.Services.AddScoped<IStockOutRepository, StockOutRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
