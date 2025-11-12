using BCrypt.Net;
using IndkoebsGenieBackend.Database.DatabaseContext;
using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.Helper;
using Microsoft.EntityFrameworkCore;

namespace IndkoebsGenieBackend.Services
{
    public class SeederService
    {
        private readonly DatabaseContext _db;

        public SeederService(DatabaseContext db)
        {
            _db = db;
        }

        public void Seed()
        {
            _db.Database.Migrate(); // sikrer at migrationer er applied

            // ----- Kendte test passwords -----
            const string adminPlain = "Admin123!";
            const string customerPlain = "Customer123!";

            var adminHash = BCrypt.Net.BCrypt.HashPassword(adminPlain);
            var customerHash = BCrypt.Net.BCrypt.HashPassword(customerPlain);

            // ----- Seed users -----
            var seedUsers = new List<User>
            {
                new User { FirstName="Admin", LastName="One", Email="admin@mail.com", Password=adminHash, Role=Role.Admin, Region="København", PostalCode=2500, Address="Admin Vej 1", City="København" },
                new User { FirstName="Admin", LastName="Two", Email="admin2@mail.com", Password=adminHash, Role=Role.Admin, Region="Sjælland", PostalCode=4000, Address="Hovedgaden 10", City="Roskilde" },
                new User { FirstName="Børge", LastName="Jeppensen", Email="testmail@mail.com", Password=customerHash, Role=Role.Customer, Region="Jylland", PostalCode=8000, Address="Test Vej 2", City="Aarhus" }
                // Tilføj flere customers her hvis du vil
            };

            foreach (var user in seedUsers)
            {
                var existing = _db.Users.SingleOrDefault(u => u.Email == user.Email);
                if (existing == null)
                {
                    _db.Users.Add(user);
                }
                else
                {
                    // Opdater password hvis det er forskelligt
                    if (existing.Password != user.Password)
                    {
                        existing.Password = user.Password;
                        _db.Users.Update(existing);
                    }
                }
            }

            _db.SaveChanges();

            // ----- Seed produkter / lister / andet data her hvis nødvendigt -----
        }
    }
}
