using IndkoebsGenieBackend.Helper;

namespace IndkoebsGenieBackend.DTO.UserDTO
{
    public class UserRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public required string Email { get; set; } = "";
        public string? Password { get; set; }
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public int PostalCode { get; set; } = 0;
        public string Region { get; set; } = "";
        public Role Role { get; set; }
    }
}
