using Employee_Management_System.Models;

public interface IEmployeeRepository
{
    Task <IEnumerable<Employee>> GetAllEmployeesAsync();
    Task <Employee> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}