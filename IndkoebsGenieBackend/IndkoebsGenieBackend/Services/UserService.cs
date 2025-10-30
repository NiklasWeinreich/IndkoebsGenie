using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.DTO.UserDTO;
using IndkoebsGenieBackend.Helper;
using IndkoebsGenieBackend.Interfaces.IUser;

namespace IndkoebsGenieBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest newUser)
        {
            var user = await _userRepository.CreateUserAsync(MapUserRequestToUser(newUser));
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            return MapUserToUserResponse(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var  deleteUser = await _userRepository.DeleteUserAsync(id);
            if (deleteUser == null)
            {
                throw new Exception("Brugeren kunne ikke slettes.");
            }
            return deleteUser;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllUsersAsync();
            if (users == null) {
                throw new Exception("Ingen brugere fundet.");
            }
            return users.Select(MapUserToUserResponse).ToList();
        }

        public async Task<UserResponse?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is null) return null;
            return MapUserToUserResponse(user);
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Brugeren blev ikke fundet.");
            }
            return MapUserToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUserByIdAsync(int userId, UserRequest updateUser)
        {
            var user = MapUserRequestToUser(updateUser);
            var insertedUser = await _userRepository.UpdateUserByIdAsync(userId, user);

            // Normalisér input
            var newEmail = updateUser.Email?.Trim().ToLowerInvariant();

            if (insertedUser != null)
            {
                return MapUserToUserResponse(insertedUser);
            }

            return null;

        }

        public static User MapUserRequestToUser(UserRequest userRequest)
        {
            User user = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                //For at hash'e password bruger vi BCrypt og trimmer whitespace
                Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password.Trim()),

                Address = userRequest.Address,
                Role = Role.Customer,
                City = userRequest.City,
                PostalCode = userRequest.PostalCode,
                Region = userRequest.Region
            };
            return user;
        }

        public static UserResponse MapUserToUserResponse(User user)
        {
            UserResponse userResponse = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                Region = user.Region
            };
            return userResponse;
        }
    }
}
