using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSuplierFromStockIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockOutCode",
                table: "StockOut");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "StockIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StockOutCode",
                table: "StockOut",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "StockIn",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 1,
                column: "Supplier",
                value: "Dell Vietnam");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 2,
                column: "Supplier",
                value: "Logitech Distributor");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 3,
                column: "Supplier",
                value: "LG Electronics");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 4,
                column: "Supplier",
                value: "Kingston Technology");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 1,
                column: "StockOutCode",
                value: "SOUT-2024-001");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 2,
                column: "StockOutCode",
                value: "SOUT-2024-002");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 3,
                column: "StockOutCode",
                value: "SOUT-2024-003");
        }
    }
}
