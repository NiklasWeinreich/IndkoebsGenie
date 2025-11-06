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
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                columns: new[] { "Id", "Address", "City", "Email", "FirstName", "LastName", "Password", "PasswordResetToken", "PostalCode", "Region", "Role", "TokenExpires" },
                values: new object[,]
                {
                    { 1, "Admin Vej 1", "København", "admin@mail.com", "Admin", "One", "$2b$12$X0tTEphJRWXToabecGex6ODPX50hK1mHpytEQ0m9TnDboK7NgWYX2", null, "2500", "København", 1, null },
                    { 2, "Hovedgaden 10", "Roskilde", "admin2@mail.com", "Admin", "Two", "$2b$12$X0tTEphJRWXToabecGex6ODPX50hK1mHpytEQ0m9TnDboK7NgWYX2", null, "4000", "Sjælland", 1, null },
                    { 3, "Test Vej 2", "Aarhus", "testmail@mail.com", "Børge", "Jeppensen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "8000", "Jylland", 0, null },
                    { 4, "Østerbrogade 45", "København", "mette.larsen@mail.com", "Mette", "Larsen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "2100", "Hovedstaden", 0, null },
                    { 5, "Algade 12", "Aalborg", "jonas.poulsen@mail.com", "Jonas", "Poulsen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "9000", "Nordjylland", 0, null },
                    { 6, "Vestergade 7", "Odense", "sofie.nielsen@mail.com", "Sofie", "Nielsen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "5000", "Syddanmark", 0, null },
                    { 7, "Byvej 3", "Silkeborg", "anders.madsen@mail.com", "Anders", "Madsen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "8600", "Midtjylland", 0, null },
                    { 8, "Parkvej 22", "Næstved", "camilla.hansen@mail.com", "Camilla", "Hansen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "4700", "Sjælland", 0, null },
                    { 9, "Havnevej 5", "Hjørring", "rasmus.christensen@mail.com", "Rasmus", "Christensen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "9800", "Nordjylland", 0, null },
                    { 10, "Torvegade 9", "Kolding", "ida.jorgensen@mail.com", "Ida", "Jørgensen", "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG", null, "6000", "Syddanmark", 0, null }
                });

            migrationBuilder.InsertData(
                table: "GroceryLists",
                columns: new[] { "Id", "CreatedAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Min første liste", 1 },
                    { 2, new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Min anden liste", 2 }
                });

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
