using Employee_Management_System.Models;
using Microsoft.EntityFrameworkCore;

public class HolidayRepository : IHolidayRepository
{
    private readonly EmployeeDbContext _employeeDbContext;

    public HolidayRepository(EmployeeDbContext employeeDbContext)
    {
        _employeeDbContext = employeeDbContext;
    }

    public async Task AddPublicHolidayAsync(PublicHoliday publicHoliday)
    {
        await _employeeDbContext.PublicHolidays.AddAsync(publicHoliday);
        await _employeeDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<PublicHoliday>> GetPublicHolidaysAsync(int year)
    {
        return await _employeeDbContext.PublicHolidays
            .Where(publicholiday=> publicholiday.HolidayDate.Year == year)
            .ToListAsync();
    }
}