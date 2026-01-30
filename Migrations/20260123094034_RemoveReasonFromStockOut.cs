using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReasonFromStockOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "StockOut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "StockOut",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 1,
                column: "Reason",
                value: "Sale");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 2,
                column: "Reason",
                value: "Sale");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 3,
                column: "Reason",
                value: "Transfer");
        }
    }
}
