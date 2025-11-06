using IndkoebsGenieBackend.Authentication;
using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.DTO.EmailDTO;
using IndkoebsGenieBackend.DTO.LoginDTO;
using IndkoebsGenieBackend.DTO.UserDTO;
using IndkoebsGenieBackend.Helper;
using IndkoebsGenieBackend.Interfaces.IEmailService;
using IndkoebsGenieBackend.Interfaces.IUser;

namespace IndkoebsGenieBackend.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IEmailService _emailService;


        public UserService(IUserRepository userRepository, IJwtUtils jwtUtils, IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _emailService = emailService;
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
            var deleted = await _userRepository.DeleteUserAsync(id);
            if (!deleted) throw new Exception("Brugeren kunne ikke slettes.");
            return true;
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

        public async Task<LoginResponse?> AuthenticateUserAsync(LoginRequest loginRequest)
        {
            var email = loginRequest.Email?.Trim();

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user is null) return null;

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                return null;

            return new LoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Token = _jwtUtils.GenerateJwtToken(user)
            };
        }


        public async Task<bool> SendPasswordResetEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            var token = Guid.NewGuid().ToString();

            user.PasswordResetToken = token;
            user.TokenExpires = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateUserByIdAsync(user.Id, user);

            //EscapeDataString for at sikre korrekt URL-formattering
            var resetLink =
                $"http://localhost:4200/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            var mail = new EmailResponse
            {
                To = user.Email,
                Subject = "Nulstil din adgangskode",
                Body = $"Hej {user.FirstName},<br><br>" +
                       $"Klik på linket her for at nulstille din adgangskode:<br>" +
                       $"<a href=\"{resetLink}\">Nulstil adgangskode</a><br><br>" +
                       "Hvis du ikke bad om dette, ignorér venligst denne mail.<br><br>" +
                       "Hilsen<br>IndKoebsGenie Teamet"
            };

            _emailService.SendEmail(mail);
            return true;
        }


        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.PasswordResetToken != token || user.TokenExpires < DateTime.UtcNow)
                return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.TokenExpires = null;
            await _userRepository.UpdateUserByIdAsync(user.Id, user);

            return true;
        }
    }
}
