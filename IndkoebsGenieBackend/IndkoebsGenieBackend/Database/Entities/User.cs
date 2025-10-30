﻿using IndkoebsGenieBackend.Helper;

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
        public string PostalCode { get; set; } = "";
        public string Region { get; set; } = "";
        public Role Role { get; set; }
        public ICollection<GroceryList> GroceryLists { get; set; } = new List<GroceryList>();
    }
}
