using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrders",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOrders", x => x.DeliveryOrderId);
                    table.ForeignKey(
                        name: "FK_DeliveryOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stocks_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockIn",
                columns: table => new
                {
                    StockInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockInCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockIn", x => x.StockInId);
                    table.ForeignKey(
                        name: "FK_StockIn_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockIn_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockIn_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOut",
                columns: table => new
                {
                    StockOutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOutCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOut", x => x.StockOutId);
                    table.ForeignKey(
                        name: "FK_StockOut_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOut_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOut_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "CreatedDate", "IsActive", "LocationCode", "LocationName", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "A-01", "Khu A - Tầng 1", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "A-02", "Khu A - Tầng 2", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "B-01", "Khu B - Tầng 1", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "C-01", "Khu C - Tầng 1", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CreatedDate", "Description", "IsActive", "ProductCode", "ProductName", "Unit", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop văn phòng, màn hình 15.6 inch", true, "PRD001", "Laptop Dell Inspiron 15", "Cái", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuột không dây, pin 24 tháng", true, "PRD002", "Chuột Logitech M331", "Cái", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bàn phím cơ RGB, switch blue", true, "PRD003", "Bàn phím Mechanical K552", "Cái", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màn hình Full HD, IPS", true, "PRD004", "Màn hình LG 24 inch", "Cái", null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "USB 3.0, tốc độ cao", true, "PRD005", "USB Kingston 32GB", "Cái", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "FullName", "IsActive", "LastLoginDate", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator", true, null, "123456", "Admin", "admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Van A", true, null, "123456", "Manager", "manager1" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tran Thi B", true, null, "123456", "Staff", "staff1" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryOrders",
                columns: new[] { "DeliveryOrderId", "CreatedDate", "DeliveryDate", "PONumber", "ProductId", "QRCode", "Quantity", "Status", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "PO-2024-001", 1, "PRD001;2024-01-15;PO-2024-001", 50, "Delivered", null },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "PO-2024-002", 2, "PRD002;2024-01-20;PO-2024-002", 200, "Delivered", null },
                    { 3, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PO-2024-003", 3, "PRD003;2024-02-01;PO-2024-003", 100, "InTransit", null }
                });

            migrationBuilder.InsertData(
                table: "StockIn",
                columns: new[] { "StockInId", "CreatedBy", "CreatedDate", "LocationId", "ProductId", "QRCode", "Quantity", "Status", "StockInCode", "Supplier", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PRD001;2024-01-15;PO-2024-001", 50, "Completed", "SIN-2024-001", "Dell Vietnam", 0m },
                    { 2, 3, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "PRD002;2024-01-20;PO-2024-002", 200, "Completed", "SIN-2024-002", "Logitech Distributor", 0m },
                    { 3, 2, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, "PRD004;2024-01-25;PO-2024-004", 30, "Completed", "SIN-2024-003", "LG Electronics", 0m },
                    { 4, 3, new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 5, "PRD005;2024-01-28;PO-2024-005", 500, "Completed", "SIN-2024-004", "Kingston Technology", 0m }
                });

            migrationBuilder.InsertData(
                table: "StockOut",
                columns: new[] { "StockOutId", "CreatedBy", "CreatedDate", "LocationId", "ProductId", "QRCode", "Quantity", "Reason", "Status", "StockOutCode" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PRD001;2024-01-15;PO-2024-001", 10, "Sale", "Completed", "SOUT-2024-001" },
                    { 2, 3, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "PRD002;2024-01-20;PO-2024-002", 50, "Sale", "Completed", "SOUT-2024-002" },
                    { 3, 2, new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 5, "PRD005;2024-01-28;PO-2024-005", 100, "Transfer", "Completed", "SOUT-2024-003" }
                });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "StockId", "LastUpdated", "LocationId", "ProductId", "QRCode", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PRD001;2024-01-15;PO-2024-001", 40 },
                    { 2, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "PRD002;2024-01-20;PO-2024-002", 150 },
                    { 3, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, "PRD004;2024-01-25;PO-2024-004", 30 },
                    { 4, new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 5, "PRD005;2024-01-28;PO-2024-005", 400 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_ProductId",
                table: "DeliveryOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIn_CreatedBy",
                table: "StockIn",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockIn_LocationId",
                table: "StockIn",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIn_ProductId",
                table: "StockIn",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_CreatedBy",
                table: "StockOut",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_LocationId",
                table: "StockOut",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_ProductId",
                table: "StockOut",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_LocationId",
                table: "Stocks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryOrders");

            migrationBuilder.DropTable(
                name: "StockIn");

            migrationBuilder.DropTable(
                name: "StockOut");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
