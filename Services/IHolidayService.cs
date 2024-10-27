using System.Threading.Tasks;
using Employee_Management_System.Models;

public interface IHolidayService
{
    Task<IEnumerable<PublicHoliday>> GetPublicHolidays(int year);
    Task AddPublicHoliday(PublicHoliday publicHoliday);
    Task<int> CalculateWorkingDays(DateTime? startDate, DateTime? endDate);
}