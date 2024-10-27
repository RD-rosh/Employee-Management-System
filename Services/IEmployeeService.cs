using Employee_Management_System.Models;

public interface IEmployeeService
{
    Task <IEnumerable<Employee>> GetAllEmployeesCached();
    Task <Employee> GetEmployeeById(int id);
    Task AddEmployee(Employee employee);
    Task UpdateEmployee(Employee employee);
    Task DeleteEmployee(int id);
}