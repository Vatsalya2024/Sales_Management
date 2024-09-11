using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sales_Management.Models;
using Sales_Management.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sales_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _userService.Authenticate(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
