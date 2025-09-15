using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.DTO;
using WebApiApplication.EmployeeApplication;

namespace WebAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeApplication _employeeApp;

        public AuthController(IEmployeeApplication employeeApp)
        {
            _employeeApp = employeeApp;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _employeeApp.RegisterAsync(dto);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _employeeApp.LoginAsync(dto);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            await _employeeApp.ChangePasswordAsync(userId, dto);
            return Ok(new { message = "Password updated" });
        }
    }
}
