using IndkoebsGenieBackend.Authentication;
using IndkoebsGenieBackend.DTO.ForgotPassword;
using IndkoebsGenieBackend.DTO.LoginDTO;
using IndkoebsGenieBackend.DTO.UserDTO;
using IndkoebsGenieBackend.Helper;
using IndkoebsGenieBackend.Interfaces.IUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndkoebsGenieBackend.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _userService.AuthenticateUserAsync(loginRequest);
            return response == null ? Unauthorized("Invalid credentials") : Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] UserRequest newUser)
        {
            var email = newUser.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email er påkrævet.");

            // normalisér input (kan også gøres i service)
            newUser.Email = email;
            newUser.Role = Role.Customer; 

            var existing = await _userService.GetUserByEmailAsync(email);
            if (existing != null)
                return Conflict("Email is already in use");

            var userResponse = await _userService.CreateUserAsync(newUser);
            return Ok(userResponse); 
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var success = await _userService.SendPasswordResetEmail(request.Email);
            if (!success) return NotFound("Bruger ikke fundet.");

            return Ok(new { message = "E-mail til nulstilling sendt." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordResponse request)
        {
            var success = await _userService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            if (!success) return BadRequest("Ugyldigt token eller e-mail.");

            return Ok(new { message = "Adgangskode nulstillet." });
        }

        [Authentication.Authorize(Role.Admin)]
        [HttpPost("admin/create-user")]
        public async Task<ActionResult<UserResponse>> AdminCreateUser([FromBody] UserRequest newUser)
        {
            var userResponse = await _userService.CreateUserAsync(newUser); // her må Role pass-through
            return Ok(userResponse);
        }
    }
}

