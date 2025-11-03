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

            // User 1..* GroceryLists
            modelBuilder.Entity<User>()
                .HasMany(u => u.GroceryLists)
                .WithOne(gl => gl.User)
                .HasForeignKey(gl => gl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.Password).HasMaxLength(100).IsUnicode(false);
            });

            // Seed data
            var seedCreatedAt = new DateTime(2025, 10, 23, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<GroceryList>().HasData(new GroceryList
            {
                Id = 1,
                Title = "Min første liste",
                CreatedAt = seedCreatedAt,
                UserId = 1 
            });

            modelBuilder.Entity<GroceryList>().HasData(new GroceryList
            {
                Id = 2,
                Title = "Min anden liste",
                CreatedAt = seedCreatedAt,
                UserId = 2
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
            // Klartekst for reference (brug KUN i dev):
            // Admin:    Passw0rd
            // Customer: Passw0rd
            const string AdminHash = "$2b$12$X0tTEphJRWXToabecGex6ODPX50hK1mHpytEQ0m9TnDboK7NgWYX2";
            const string CustomerHash = "$2b$12$p/4Pfi4v6xFRMp9F.WGfCeGDLB2/JddhCeL9C5/n5GVaojZltZjSG";


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "One",
                    Email = "admin@mail.com",
                    Password = AdminHash,
                    Role = Role.Admin,
                    Region = "København",
                    PostalCode = "2500",
                    Address = "Admin Vej 1",
                    City = "København"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Admin",
                    LastName = "Two",
                    Email = "admin2@mail.com",
                    Password = AdminHash,
                    Role = Role.Admin,
                    Region = "Sjælland",
                    PostalCode = "4000",
                    Address = "Hovedgaden 10",
                    City = "Roskilde"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Børge",
                    LastName = "Jeppensen",
                    Email = "testmail@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Jylland",
                    PostalCode = "8000",
                    Address = "Test Vej 2",
                    City = "Aarhus"
                },
                new User
                {
                    Id = 4,
                    FirstName = "Mette",
                    LastName = "Larsen",
                    Email = "mette.larsen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Hovedstaden",
                    PostalCode = "2100",
                    Address = "Østerbrogade 45",
                    City = "København"
                },
                new User
                {
                    Id = 5,
                    FirstName = "Jonas",
                    LastName = "Poulsen",
                    Email = "jonas.poulsen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Nordjylland",
                    PostalCode = "9000",
                    Address = "Algade 12",
                    City = "Aalborg"
                },
                new User
                {
                    Id = 6,
                    FirstName = "Sofie",
                    LastName = "Nielsen",
                    Email = "sofie.nielsen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Syddanmark",
                    PostalCode = "5000",
                    Address = "Vestergade 7",
                    City = "Odense"
                },
                new User
                {
                    Id = 7,
                    FirstName = "Anders",
                    LastName = "Madsen",
                    Email = "anders.madsen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Midtjylland",
                    PostalCode = "8600",
                    Address = "Byvej 3",
                    City = "Silkeborg"
                },
                new User
                {
                    Id = 8,
                    FirstName = "Camilla",
                    LastName = "Hansen",
                    Email = "camilla.hansen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Sjælland",
                    PostalCode = "4700",
                    Address = "Parkvej 22",
                    City = "Næstved"
                },
                new User
                {
                    Id = 9,
                    FirstName = "Rasmus",
                    LastName = "Christensen",
                    Email = "rasmus.christensen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Nordjylland",
                    PostalCode = "9800",
                    Address = "Havnevej 5",
                    City = "Hjørring"
                },
                new User
                {
                    Id = 10,
                    FirstName = "Ida",
                    LastName = "Jørgensen",
                    Email = "ida.jorgensen@mail.com",
                    Password = CustomerHash,
                    Role = Role.Customer,
                    Region = "Syddanmark",
                    PostalCode = "6000",
                    Address = "Torvegade 9",
                    City = "Kolding"
                }
            );

        }
    }
}
