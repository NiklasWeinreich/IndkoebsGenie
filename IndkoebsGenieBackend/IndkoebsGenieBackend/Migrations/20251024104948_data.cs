using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IndkoebsGenieBackend.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroceryLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    GroceryListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItems_GroceryLists_GroceryListId",
                        column: x => x.GroceryListId,
                        principalTable: "GroceryLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GroceryLists",
                columns: new[] { "Id", "CreatedAt", "Title" },
                values: new object[] { 1, new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Min første liste" });

            migrationBuilder.InsertData(
                table: "ProductItems",
                columns: new[] { "Id", "Category", "GroceryListId", "IsCompleted", "Name", "Notes", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Mælk", "Letmælk", 2 },
                    { 2, 2, 1, false, "Brød", "Fuldkorn", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_GroceryListId_IsCompleted",
                table: "ProductItems",
                columns: new[] { "GroceryListId", "IsCompleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "GroceryLists");
        }
    }
}
