using IndkoebsGenieBackend.Database.Entities;
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
        Task<bool> DeleteUserAsync(int id);
    }
}
