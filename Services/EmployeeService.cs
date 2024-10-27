using Employee_Management_System.Models;
using Microsoft.Extensions.Caching.Memory;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMemoryCache _memoryCache;

    public EmployeeService(IEmployeeRepository employeeRepository, IMemoryCache memoryCache)
    {
        _employeeRepository = employeeRepository;
        _memoryCache = memoryCache;
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        return await _employeeRepository.GetEmployeeByIdAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesCached()
    {
        return await Cached(() => _employeeRepository.GetAllEmployeesAsync(), "employeesList", TimeSpan.FromMinutes(5));
    }

    public async Task UpdateEmployee(Employee employee)
    {
        await _employeeRepository.UpdateEmployeeAsync(employee);
        _memoryCache.Remove("employeesList");
    }

    public async Task AddEmployee(Employee employee)
    {
        await _employeeRepository.AddEmployeeAsync(employee);
        _memoryCache.Remove("employeesList");
    }

    public async Task DeleteEmployee(int id)
    {
        await _employeeRepository.DeleteEmployeeAsync(id);
        _memoryCache.Remove("employeesList");
    }

    private async Task<T> Cached<T>(Func<Task<T>> getData, string cacheKey, TimeSpan expiration)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out T cachedData))
        {
            cachedData = await getData();
            _memoryCache.Set(cacheKey, cachedData, expiration);
        }
        return cachedData;
    }

    public void CachedLong<T>(T item, string cacheKey)
    {
        _memoryCache.Set(cacheKey, item);
    }

    public void RemoveCachedLong(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }
}
