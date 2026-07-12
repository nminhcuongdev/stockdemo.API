using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductMinMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxQuantity",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "MaxQuantity", "MinQuantity" },
                values: new object[] { 100, 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "MaxQuantity", "MinQuantity" },
                values: new object[] { null, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "MaxQuantity", "MinQuantity" },
                values: new object[] { null, 20 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "MaxQuantity", "MinQuantity" },
                values: new object[] { null, 40 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "MaxQuantity", "MinQuantity" },
                values: new object[] { null, 500 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "Products");
        }
    }
}
