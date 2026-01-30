using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalAmountFromStockIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "StockIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "StockIn",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 1,
                column: "TotalAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 2,
                column: "TotalAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 3,
                column: "TotalAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 4,
                column: "TotalAmount",
                value: 0m);
        }
    }
}
