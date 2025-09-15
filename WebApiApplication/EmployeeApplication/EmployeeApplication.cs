using WebApiApplication.Interfaces;
using WebApiData.Repository;
using WebApiDomain.Model;

namespace WebApiApplication.Services
{
    public class EmployeeApplication : IEmployeeApplication
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeApplication(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            return await _employeeRepo.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task RegisterAsync(Employee employee)
        {
            await _employeeRepo.AddAsync(employee);
            await _employeeRepo.SaveAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _employeeRepo.Update(employee);
            await _employeeRepo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _employeeRepo.GetByIdAsync(id);
            if (emp != null)
            {
                _employeeRepo.Delete(emp);
                await _employeeRepo.SaveAsync();
            }
        }
    }
}
