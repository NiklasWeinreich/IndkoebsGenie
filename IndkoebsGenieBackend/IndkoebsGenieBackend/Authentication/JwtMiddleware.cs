using IndkoebsGenieBackend.Interfaces.IUser;
using IndkoebsGenieBackend.Services.UserService;

namespace IndkoebsGenieBackend.Authentication
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _jwt;

        public JwtMiddleware(RequestDelegate jwt)
        {
            _jwt = jwt;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            int? Id = jwtUtils.ValidateJwtToken(token);
            if (Id is not null)
            {
                //Attach account to context on succesful jwt validation
                var user = await userRepository.GetUserByIdAsync(Id.Value);
                if (user != null)
                {
                    context.Items["User"] = UserService.MapUserToUserResponse(user);
                }
            }

            await _jwt(context);
        }
    }
}
