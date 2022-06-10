using HotDesk.Models;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IHotDeskRepository _repository;
        public UserController(IHotDeskRepository hotDeskRepository)
        {
            _repository = hotDeskRepository;
        }

        [Authorize]
        [HttpPatch("SetRole")]
        public IActionResult SetRole([FromBody] string role)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                user.Role = role;
                _repository.UpdateUser(user.Username,role);
                return Ok(user);
            }
            return NotFound("User not found");
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}