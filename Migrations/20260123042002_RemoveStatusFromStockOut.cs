using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusFromStockOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StockOut");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StockIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StockOut",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StockIn",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 1,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 2,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 3,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockIn",
                keyColumn: "StockInId",
                keyValue: 4,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 1,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 2,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockOut",
                keyColumn: "StockOutId",
                keyValue: 3,
                column: "Status",
                value: "Completed");
        }
    }
}
