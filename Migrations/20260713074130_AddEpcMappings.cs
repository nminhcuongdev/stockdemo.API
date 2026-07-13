using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockDemo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEpcMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EpcMappings",
                columns: table => new
                {
                    Epc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    MappedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpcMappings", x => x.Epc);
                    table.ForeignKey(
                        name: "FK_EpcMappings_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpcMappings_StockId",
                table: "EpcMappings",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpcMappings");
        }
    }
}
