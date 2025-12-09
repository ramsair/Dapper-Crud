using Dapper_Crud.Models;
using Dapper_Crud.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        private readonly Dictionary<string, string> _users = new()
        {
            { "sai", "1234" },
            { "admin", "admin123" }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            if (_users.TryGetValue(model.Username, out var pwd) && pwd == model.Password)
            {
                var token = _tokenService.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : null;

            if (token != null)
            {
                _tokenService.InvalidateToken(token);
                return Ok("Logged out successfully");
            }

            return BadRequest("Token not found");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var username = User.Identity?.Name;
            return Ok(new { Message = $"Welcome {username}", Time = DateTime.UtcNow });
        }
    }
}
//// Mock database
//private readonly List<UserInfo> _users = new()
//        {
//            new UserInfo { Username = "sai", Password = "1234", Role = "Admin", Department = "IT" },
//            new UserInfo { Username = "john", Password = "5678", Role = "Employee", Department = "HR" }
//        };

//[HttpPost("login")]
//public IActionResult Login([FromBody] UserLogin model)
//{
//    var user = _users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
//    if (user == null)
//        return Unauthorized("Invalid credentials");

//    var token = _tokenService.GenerateToken(user);
//    return Ok(new { Token = token, Role = user.Role });
//}