using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.DTO.LoginDTO;
using IndkoebsGenieBackend.DTO.UserDTO;

namespace IndkoebsGenieBackend.Interfaces.IUser
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<UserResponse> CreateUserAsync(UserRequest newUser);
        Task<UserResponse> UpdateUserByIdAsync(int userId, UserRequest updateUser);
        Task<LoginResponse?> AuthenticateUserAsync(LoginRequest loginRequest);
        Task<bool> SendPasswordResetEmail(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> DeleteUserAsync(int id);
    }
}
