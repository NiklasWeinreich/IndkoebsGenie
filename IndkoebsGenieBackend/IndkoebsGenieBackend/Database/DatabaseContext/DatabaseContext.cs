using IndkoebsGenieBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndkoebsGenieBackend.Database.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<GroceryList> GroceryLists { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryList>()
                .HasMany(gl => gl.Items)
                .WithOne(pi => pi.GroceryList!)
                .HasForeignKey(pi => pi.GroceryListId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProductItem>()
                .HasIndex(p => new { p.GroceryListId, p.IsCompleted });

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

                    Category = default, 
                    GroceryListId = 1
                },
                new ProductItem
                {
                    Id = 2,
                    Name = "Brød",
                    Quantity = 2,
                    Notes = "Fuldkorn",
                    IsCompleted = false,
                    Category = default, 
                    GroceryListId = 1
                }
            );
        }
    }
}
