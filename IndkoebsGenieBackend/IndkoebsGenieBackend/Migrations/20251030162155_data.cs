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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroceryLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Users",
                columns: new[] { "Id", "Address", "City", "Email", "FirstName", "LastName", "Password", "PostalCode", "Region", "Role" },
                values: new object[,]
                {
                    { 1, "Admin vej 1", "AdminBy", "Admin@mail.com", "Admin", "Admin", "$2b$12$X0tTEphJRWXToabecGex6ODPX50hK1mHpytEQ0m9TnDboK7NgWYX2", "2500", "København", 1 },
                    { 2, "Test Vej 2", "Test By", "testmail@mail.com", "Børge", "Jeppensen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", "8000", "Jylland", 0 }
                });

            migrationBuilder.InsertData(
                table: "GroceryLists",
                columns: new[] { "Id", "CreatedAt", "Title", "UserId" },
                values: new object[] { 1, new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Min første liste", 2 });

            migrationBuilder.InsertData(
                table: "ProductItems",
                columns: new[] { "Id", "Category", "GroceryListId", "IsCompleted", "Name", "Notes", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Mælk", "Letmælk", 2 },
                    { 2, 2, 1, false, "Brød", "Fuldkorn", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryLists_UserId",
                table: "GroceryLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_GroceryListId_IsCompleted",
                table: "ProductItems",
                columns: new[] { "GroceryListId", "IsCompleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "GroceryLists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
