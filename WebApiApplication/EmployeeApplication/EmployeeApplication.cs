using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiApplication.DTO;
using WebApiData;
using WebApiDomain.Model;

namespace WebApiApplication.EmployeeApplication
{
    public class EmployeeApplication : IEmployeeApplication
    {
        private readonly DataContext _db;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<Employee> _hasher;

        public EmployeeApplication(DataContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
            _hasher = new PasswordHasher<Employee>();
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            if (await _db.Employee.AnyAsync(e => e.Email == dto.Email))
                throw new Exception("Email already exists");

            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                RoleId = 3,
                IsEnabled = "true"
            }; 

            employee.PasswordHash = _hasher.HashPassword(employee, dto.Password);

            _db.Employee.Add(employee);
            await _db.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _db.Employee.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == dto.Email);
            if (user == null) throw new Exception("Invalid credentials");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) throw new Exception("Invalid credentials");

            if (!Convert.ToBoolean(user.IsEnabled)) throw new Exception("User disabled by Admin");

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = await _db.Employee.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
            if (result == PasswordVerificationResult.Failed) throw new Exception("Old password incorrect");

            user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);
            await _db.SaveChangesAsync();
        }
    }
}
