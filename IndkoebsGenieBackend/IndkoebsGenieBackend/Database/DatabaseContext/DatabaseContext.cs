using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.Helper;
using Microsoft.EntityFrameworkCore;

namespace IndkoebsGenieBackend.Database.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<GroceryList> GroceryLists { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // GroceryList - ProductItem relation
            modelBuilder.Entity<GroceryList>()
                .HasMany(gl => gl.Items)
                .WithOne(pi => pi.GroceryList!)
                .HasForeignKey(pi => pi.GroceryListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unik email for User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index til filtrering af items
            modelBuilder.Entity<ProductItem>()
                .HasIndex(p => new { p.GroceryListId, p.IsCompleted });

            // Seed data
            var seedCreatedAt = new DateTime(2025, 10, 23, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<GroceryList>().HasData(new GroceryList
            {
                Id = 1,
                Title = "Min første liste",
                CreatedAt = seedCreatedAt
            });

            modelBuilder.Entity<ProductItem>().HasData(
                new ProductItem
                {
                    Id = 1,
                    Name = "Mælk",
                    Quantity = 2,
                    Notes = "Letmælk",
                    IsCompleted = false,
                    Category = ProductCategory.Dairy,
                    GroceryListId = 1
                },
                new ProductItem
                {
                    Id = 2,
                    Name = "Brød",
                    Quantity = 2,
                    Notes = "Fuldkorn",
                    IsCompleted = false,
                    Category = ProductCategory.Bakery,
                    GroceryListId = 1
                }
            );

            // ----- Statisk bcrypt-hash (genereret én gang for stabil EF-model) -----
            // Klartekst for reference:
            // Admin:    Passw0rd
            // Customer: Passw0rd

            const string AdminHash = "$2b$12$X0tTEphJRWXToabecGex6ODPX50hK1mHpytEQ0m9TnDboK7NgWYX2";
            const string CustomerHash = "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG";

            // Seed Users (Admin + Customer)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "Admin@mail.com",
                    Password = AdminHash,
                    Role = Role.Admin,
                    Region = "København",
                    PostalCode = "2500",
                    Address = "Admin vej 1",
                    City = "AdminBy"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Børge",
                    LastName = "Jeppensen",
                    Email = "testmail@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Jylland",
                    PostalCode = "8000",
                    Address = "Test Vej 2",
                    City = "Test By"
                }
            );
        }
    }
}
