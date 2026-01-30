using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStockInCodeFromStockIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockInCode",
                table: "StockIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StockInCode",
                table: "StockIn",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 1,
                column: "StockInCode",
                value: "SIN-2024-001");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 2,
                column: "StockInCode",
                value: "SIN-2024-002");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 3,
                column: "StockInCode",
                value: "SIN-2024-003");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 4,
                column: "StockInCode",
                value: "SIN-2024-004");
        }
    }
}
