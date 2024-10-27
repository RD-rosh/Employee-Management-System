using Employee_Management_System.Models;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _employeeDbContext;

    public EmployeeRepository(EmployeeDbContext employeeDbContext)
    {
        _employeeDbContext = employeeDbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _employeeDbContext.Employees.ToListAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await _employeeDbContext.Employees.FindAsync(id);
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        _employeeDbContext.Employees.Add(employee);
        await _employeeDbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _employeeDbContext.Employees.Update(employee);
        await _employeeDbContext.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _employeeDbContext.Employees.FindAsync(id);
        if (employee != null)
        {
            _employeeDbContext.Employees.Remove(employee);
            await _employeeDbContext.SaveChangesAsync();
        }
        
    }
}