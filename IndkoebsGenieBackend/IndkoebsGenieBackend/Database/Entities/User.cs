using IndkoebsGenieBackend.Helper;

namespace IndkoebsGenieBackend.Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public required string Email { get; set; } = "";
        public required string Password { get; set; }
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public int PostalCode { get; set; } = 0;
        public string Region { get; set; } = "";
        public Role Role { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpires { get; set; }
        public ICollection<GroceryList> GroceryLists { get; set; } = new List<GroceryList>();
    }
}
