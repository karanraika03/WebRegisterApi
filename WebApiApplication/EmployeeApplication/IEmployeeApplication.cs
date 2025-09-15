using WebApiDomain.Model;

namespace WebApiApplication.Interfaces
{
    public interface IEmployeeApplication
    {
        Task<Employee?> GetEmployeeByEmail(string email);
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task RegisterAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
