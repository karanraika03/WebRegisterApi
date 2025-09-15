using Microsoft.AspNetCore.Mvc;
using WebApiDomain.Model;

namespace WebAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private static List<Role> _roles = new();

        [HttpPost("register")]
        public IActionResult Register(Role role)
        {
            if (_roles.Any(x => x.Name == role.Name))
                return BadRequest("User already exists");

            _roles.Add(role);
            return Ok("User registered successfully");
        }
    }
}


