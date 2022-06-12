using HotDesk.Models;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IHotDeskRepository _repository;

        public LoginController(IHotDeskRepository hotDeskRepository)
        {
            _repository = hotDeskRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserLogin userModel)
        {
            var user = userModel;

            if (user != null)
            {
                try
                {
                    _repository.AddUser(user);
                    return Ok(user);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }
        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConsts.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(JWTConsts.Issuer,
              JWTConsts.Audience,
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = _repository.GetUser(userLogin);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
        public static class JWTConsts
        {
            public const string Issuer = "https://localhost:44381/";
            public const string Audience = "https://localhost:44381/";
            public const string Key = "DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4";
        }
    }
}