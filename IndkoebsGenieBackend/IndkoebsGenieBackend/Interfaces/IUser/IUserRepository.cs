using IndkoebsGenieBackend.Database.Entities;

namespace IndkoebsGenieBackend.Interfaces.IUser
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User newUser);
        Task<User> UpdateUserByIdAsync(int userId, User updateUser);
        Task<bool> DeleteUserAsync(int id);
    }
}
