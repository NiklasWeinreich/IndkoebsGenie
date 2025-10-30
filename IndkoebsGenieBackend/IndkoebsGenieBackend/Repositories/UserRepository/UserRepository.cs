using IndkoebsGenieBackend.Database.DatabaseContext;
using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.Interfaces.IUser;
using Microsoft.EntityFrameworkCore;

namespace IndkoebsGenieBackend.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {

        private readonly DatabaseContext _dbcontext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _dbcontext = databaseContext;
        }

        public async Task<User> CreateUserAsync(User newUser)
        {
            _dbcontext.Users.Add(newUser);
            await _dbcontext.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbcontext.Users.FindAsync(id);
            if (user == null) return false;

            _dbcontext.Users.Remove(user);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbcontext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await _dbcontext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbcontext.Users.FindAsync(id);
        }

        public async Task<User> UpdateUserByIdAsync(int userId, User updateUser)
        {
            var user = await _dbcontext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Brugeren blev ikke fundet.");
            }
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Email = updateUser.Email;
            user.Address = updateUser.Address;
            user.City = updateUser.City;
            user.PostalCode = updateUser.PostalCode;
            user.Region = updateUser.Region;
            _dbcontext.Users.Update(user);

            await _dbcontext.SaveChangesAsync();

            return user;
        }
    }
}
