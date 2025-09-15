using WebApiApplication.DTO;

namespace WebApiApplication.EmployeeApplication
{
    public interface IEmployeeApplication
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
