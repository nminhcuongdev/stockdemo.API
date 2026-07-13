using Microsoft.EntityFrameworkCore;
using StockDemo.API.Models.Domain;

namespace StockDemo.API.Data
{
    public class StockDemoDbContext: DbContext
    {
        public StockDemoDbContext(DbContextOptions<StockDemoDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTake> StockTakes { get; set; }
        public DbSet<StockTakeItem> StockTakeItems { get; set; }
        public DbSet<EpcMapping> EpcMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // A transfer references two locations; disable cascade delete to avoid
            // SQL Server "multiple cascade paths" and to keep transfer history intact.
            modelBuilder.Entity<StockTransfer>(entity =>
            {
                entity.HasOne(t => t.FromLocation)
                    .WithMany()
                    .HasForeignKey(t => t.FromLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ToLocation)
                    .WithMany()
                    .HasForeignKey(t => t.ToLocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Product)
                    .WithMany()
                    .HasForeignKey(t => t.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockTake>(entity =>
            {
                entity.HasOne(s => s.Location)
                    .WithMany()
                    .HasForeignKey(s => s.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.User)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockTakeItem>(entity =>
            {
                entity.HasOne(i => i.StockTake)
                    .WithMany(s => s.Items)
                    .HasForeignKey(i => i.StockTakeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Product)
                    .WithMany()
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed EPC mappings: physical RFID tags (EPC = 'A' + 20 zeros + 3 digits)
            // paired with existing seeded stocks (StockId 1..4), assigned at random.
            modelBuilder.Entity<EpcMapping>().HasData(
                new EpcMapping { Epc = "A00000000000000000000250", StockId = 1, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000251", StockId = 3, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000252", StockId = 2, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000253", StockId = 4, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000264", StockId = 1, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000265", StockId = 2, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000266", StockId = 4, MappedDate = new DateTime(2024, 1, 1) },
                new EpcMapping { Epc = "A00000000000000000000267", StockId = 3, MappedDate = new DateTime(2024, 1, 1) }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = "vKjULaJ033cQ76FN/YbnpW0cLjFZWmecQtQGjO5QgQZt8Pxoj8J3JTjEALz4NDnA",   
                    FullName = "Administrator",
                    Role = "Admin",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserId = 2,
                    Username = "manager1",
                    PasswordHash = "vKjULaJ033cQ76FN/YbnpW0cLjFZWmecQtQGjO5QgQZt8Pxoj8J3JTjEALz4NDnA",
                    FullName = "Nguyen Van A",
                    Role = "Manager",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserId = 3,
                    Username = "staff1",
                    PasswordHash = "vKjULaJ033cQ76FN/YbnpW0cLjFZWmecQtQGjO5QgQZt8Pxoj8J3JTjEALz4NDnA",
                    FullName = "Tran Thi B",
                    Role = "Staff",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Seed Locations
            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    LocationId = 1,
                    LocationCode = "A-01",
                    LocationName = "Khu A - Tầng 1",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Location
                {
                    LocationId = 2,
                    LocationCode = "A-02",
                    LocationName = "Khu A - Tầng 2",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Location
                {
                    LocationId = 3,
                    LocationCode = "B-01",
                    LocationName = "Khu B - Tầng 1",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Location
                {
                    LocationId = 4,
                    LocationCode = "C-01",
                    LocationName = "Khu C - Tầng 1",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductCode = "PRD001",
                    MinQuantity = 45,
                    MaxQuantity = 100,
                    ProductName = "Laptop Dell Inspiron 15",
                    Description = "Laptop văn phòng, màn hình 15.6 inch",
                    Unit = "Cái",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    ProductId = 2,
                    ProductCode = "PRD002",
                    MinQuantity = 50,
                    ProductName = "Chuột Logitech M331",
                    Description = "Chuột không dây, pin 24 tháng",
                    Unit = "Cái",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    ProductId = 3,
                    ProductCode = "PRD003",
                    MinQuantity = 20,
                    ProductName = "Bàn phím Mechanical K552",
                    Description = "Bàn phím cơ RGB, switch blue",
                    Unit = "Cái",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    ProductId = 4,
                    ProductCode = "PRD004",
                    MinQuantity = 40,
                    ProductName = "Màn hình LG 24 inch",
                    Description = "Màn hình Full HD, IPS",
                    Unit = "Cái",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Product
                {
                    ProductId = 5,
                    ProductCode = "PRD005",
                    MinQuantity = 500,
                    ProductName = "USB Kingston 32GB",
                    Description = "USB 3.0, tốc độ cao",
                    Unit = "Cái",
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Seed DeliveryOrders
            modelBuilder.Entity<DeliveryOrder>().HasData(
                new DeliveryOrder
                {
                    DeliveryOrderId = 1,
                    ProductId = 1,
                    PONumber = "PO-2024-001",
                    DeliveryDate = new DateTime(2024, 1, 15),
                    Quantity = 50,
                    QRCode = "PRD001;2024-01-15;PO-2024-001",
                    Status = "Delivered",
                    CreatedDate = new DateTime(2024, 1, 10)
                },
                new DeliveryOrder
                {
                    DeliveryOrderId = 2,
                    ProductId = 2,
                    PONumber = "PO-2024-002",
                    DeliveryDate = new DateTime(2024, 1, 20),
                    Quantity = 200,
                    QRCode = "PRD002;2024-01-20;PO-2024-002",
                    Status = "Delivered",
                    CreatedDate = new DateTime(2024, 1, 15)
                },
                new DeliveryOrder
                {
                    DeliveryOrderId = 3,
                    ProductId = 3,
                    PONumber = "PO-2024-003",
                    DeliveryDate = new DateTime(2024, 2, 1),
                    Quantity = 100,
                    QRCode = "PRD003;2024-02-01;PO-2024-003",
                    Status = "InTransit",
                    CreatedDate = new DateTime(2024, 1, 25)
                }
            );

            // Seed StockIns
            modelBuilder.Entity<StockIn>().HasData(
                new StockIn
                {
                    StockInId = 1,
                    ProductId = 1,
                    LocationId = 1,
                    Quantity = 50,
                    QRCode = "PRD001;2024-01-15;PO-2024-001",
                    CreatedBy = 2,
                    CreatedDate = new DateTime(2024, 1, 15)
                },
                new StockIn
                {
                    StockInId = 2,
                    ProductId = 2,
                    LocationId = 2,
                    Quantity = 200,
                    QRCode = "PRD002;2024-01-20;PO-2024-002",
                    CreatedBy = 3,
                    CreatedDate = new DateTime(2024, 1, 20)
                },
                new StockIn
                {
                    StockInId = 3,
                    ProductId = 4,
                    LocationId = 3,
                    Quantity = 30,
                    QRCode = "PRD004;2024-01-25;PO-2024-004",
                    CreatedBy = 2,
                    CreatedDate = new DateTime(2024, 1, 25),
                },
                new StockIn
                {
                    StockInId = 4,
                    ProductId = 5,
                    LocationId = 4,
                    Quantity = 500,
                    QRCode = "PRD005;2024-01-28;PO-2024-005",
                    CreatedBy = 3,
                    CreatedDate = new DateTime(2024, 1, 28),
                }
            );

            // Seed StockOuts
            modelBuilder.Entity<StockOut>().HasData(
                new StockOut
                {
                    StockOutId = 1,
                    ProductId = 1,
                    LocationId = 1,
                    Quantity = 10,
                    QRCode = "PRD001;2024-01-15;PO-2024-001",
                    CreatedBy = 3,
                    CreatedDate = new DateTime(2024, 1, 18),
                },
                new StockOut
                {
                    StockOutId = 2,
                    ProductId = 2,
                    LocationId = 2,
                    Quantity = 50,
                    QRCode = "PRD002;2024-01-20;PO-2024-002",
                    CreatedBy = 3,
                    CreatedDate = new DateTime(2024, 1, 22),
                },
                new StockOut
                {
                    StockOutId = 3,
                    ProductId = 5,
                    LocationId = 4,
                    Quantity = 100,
                    QRCode = "PRD005;2024-01-28;PO-2024-005",
                    CreatedBy = 2,
                    CreatedDate = new DateTime(2024, 1, 30),
                }
            );

            // Seed Stocks
            modelBuilder.Entity<Stock>().HasData(
                new Stock
                {
                    StockId = 1,
                    ProductId = 1,
                    LocationId = 1,
                    Quantity = 40, // 50 nhập - 10 xuất
                    QRCode = "PRD001;2024-01-15;PO-2024-001",
                    LastUpdated = new DateTime(2024, 1, 18)
                },
                new Stock
                {
                    StockId = 2,
                    ProductId = 2,
                    LocationId = 2,
                    Quantity = 150, // 200 nhập - 50 xuất
                    QRCode = "PRD002;2024-01-20;PO-2024-002",
                    LastUpdated = new DateTime(2024, 1, 22)
                },
                new Stock
                {
                    StockId = 3,
                    ProductId = 4,
                    LocationId = 3,
                    Quantity = 30, // 30 nhập - 0 xuất
                    QRCode = "PRD004;2024-01-25;PO-2024-004",
                    LastUpdated = new DateTime(2024, 1, 25)
                },
                new Stock
                {
                    StockId = 4,
                    ProductId = 5,
                    LocationId = 4,
                    Quantity = 400, // 500 nhập - 100 xuất
                    QRCode = "PRD005;2024-01-28;PO-2024-005",
                    LastUpdated = new DateTime(2024, 1, 30)
                }
            );
        }
    }
}
