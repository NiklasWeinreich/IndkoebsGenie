using IndkoebsGenieBackend.DTO.UserDTO;
using IndkoebsGenieBackend.Interfaces.IUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndkoebsGenieBackend.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                // Kalder service-laget for at hente alle brugere fra databasen
                var users = await _userService.GetAllUsersAsync();

                // Hvis listen er null eller tom, returneres en 204 No Content
                if (users == null || users.Count == 0)
                    return NoContent();

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Hvis der opstår en uventet fejl (fx databasefejl)
                return Problem($"Der opstod en fejl: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int userId)
        {
            try
            {
                UserResponse userResponse = await _userService.GetUserByIdAsync(userId);

                if (userResponse == null)
                {
                    return NotFound();
                }
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("email")]
        public async Task<IActionResult> FindByEmailAsync([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user is null) return NotFound(new { message = "Bruger blev ikke fundet." });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int userId, [FromBody] UserRequest updateUser)
        {
            try
            {
                var userResponse = await _userService.UpdateUserByIdAsync(userId, updateUser);

                if (userResponse == null)
                {
                    return NotFound();
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int userId)
        {
            try
            {
                var userResponse = await _userService.DeleteUserAsync(userId);
                if (userResponse == null)
                {
                    return NotFound();
                }
                return Ok(userResponse);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateAsync([FromBody] UserRequest newUser)
        {
            try
            {
                var mail = await _userService.GetUserByEmailAsync(newUser.Email);
                if (mail != null)
                {
                    return Conflict("Email is already in use");
                }

                UserResponse userResponse = await _userService.CreateUserAsync(newUser);

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{userId}/lists")]
        public async Task<IActionResult> GetListsByUserId([FromRoute] int userId)
        {
            try
            {
                // Henter brugeren inkl. deres lister
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound($"Bruger med ID {userId} blev ikke fundet.");

                // Hvis brugeren ikke har nogen lister
                if (user.GroceryLists == null || !user.GroceryLists.Any())
                    return NoContent();

                // Returner kun nødvendige data (DTO-lignende anonymt objekt)
                var lists = user.GroceryLists.Select(gl => new
                {
                    gl.Id,
                    gl.Title,
                    gl.CreatedAt
                });

                return Ok(lists);
            }
            catch (Exception ex)
            {
                return Problem($"Der opstod en fejl ved hentning af lister: {ex.Message}");
            }
        }

    }
}
