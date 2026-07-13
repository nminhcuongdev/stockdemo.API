using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedEpcMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EpcMappings",
                columns: new[] { "Epc", "MappedDate", "StockId" },
                values: new object[,]
                {
                    { "A00000000000000000000250", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { "A00000000000000000000251", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { "A00000000000000000000252", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { "A00000000000000000000253", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { "A00000000000000000000264", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { "A00000000000000000000265", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { "A00000000000000000000266", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { "A00000000000000000000267", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000250");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000251");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000252");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000253");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000264");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000265");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000266");

            migrationBuilder.DeleteData(
                table: "EpcMappings",
                keyColumn: "Epc",
                keyValue: "A00000000000000000000267");
        }
    }
}
