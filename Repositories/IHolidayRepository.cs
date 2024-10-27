
using Employee_Management_System.Models;

public interface IHolidayRepository
{
    Task<IEnumerable<PublicHoliday>> GetPublicHolidaysAsync(int year);
    Task AddPublicHolidayAsync(PublicHoliday publicHoliday);
}