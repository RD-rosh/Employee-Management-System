using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Employee_Management_System.Models;
using System.Linq;

public class HolidayService : IHolidayService
{
    private readonly EmployeeDbContext _employeeDbContext;
    private readonly HttpClient _httpClient;

    //enter relevant calendarific APIkey
    private readonly string _calendarificApiKey = "ApiKey";

    public HolidayService(EmployeeDbContext employeeDbContext, HttpClient httpClient)
    {
        _employeeDbContext = employeeDbContext;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PublicHoliday>> GetPublicHolidays(int year)
    {
        try
        {
            var cachedHolidays = await GetCachedHolidays(year);
            if (cachedHolidays.Any())
            {
                return cachedHolidays;
            }

            //use of calendarific Api to fetch holidays
            var url = $"https://calendarific.com/api/v2/holidays?api_key={_calendarificApiKey}&country=lk&year={year}&type=national";
            var response = await _httpClient.GetStringAsync(url);
            var holidays = ParseHolidays(response);
            await SaveHolidaysToDatabase(holidays);
            return holidays;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching holidays: {ex.Message}");
            return new List<PublicHoliday>();
        }
    }

    private async Task<List<PublicHoliday>> GetCachedHolidays(int year)
    {
        return await _employeeDbContext.PublicHolidays
            .Where(h => h.HolidayDate.Year == year)
            .ToListAsync();
    }

    private List<PublicHoliday> ParseHolidays(string jsonResponse)
    {
        var holidays = new List<PublicHoliday>();
        //from the json output taken from api call, is deserialized to save into db

        var parsedResponse = JsonSerializer.Deserialize<HolidayResponse>(jsonResponse);
        if (parsedResponse?.Response?.Holidays != null)
        {
            foreach (var holidayData in parsedResponse.Response.Holidays)
            {
                holidays.Add(new PublicHoliday
                {
                    HolidayDate = DateTime.Parse(holidayData.Date.Iso).ToUniversalTime(),
                    HolidayName = holidayData.Name
                });
            }
        }

        return holidays;
    }

    private async Task SaveHolidaysToDatabase(List<PublicHoliday> holidays)
    {
        foreach (var holiday in holidays)
        {
            var holidayExists = await _employeeDbContext.PublicHolidays.AnyAsync(h => h.HolidayDate == holiday.HolidayDate);
            if (!holidayExists)
            {
                await _employeeDbContext.PublicHolidays.AddAsync(holiday);
            }
        }

        await _employeeDbContext.SaveChangesAsync();
    }

    public async Task AddPublicHoliday(PublicHoliday publicHoliday)
    {
        _employeeDbContext.PublicHolidays.Add(publicHoliday);
        await _employeeDbContext.SaveChangesAsync();
    }

    public async Task<int> CalculateWorkingDays(DateTime? startDate = null, DateTime? endDate = null)
    {
        //Dates in UTC format conversion
        DateTime end = endDate?.ToUniversalTime() ?? DateTime.UtcNow.Date;
        DateTime start = startDate?.ToUniversalTime() ?? end.AddDays(-7);

        var yearsRange = Enumerable.Range(start.Year, end.Year - start.Year + 1);
        foreach (int year in yearsRange)
        {
            var cachedHolidays = await GetCachedHolidays(year);
            if (!cachedHolidays.Any())
            {
                await GetPublicHolidays(year);
            }
        }

        int workingDays = 0;
        var publicHolidays = await _employeeDbContext.PublicHolidays
        //between the selected start date to end date number of public holidays in betwwen calculated
            .Where(h => h.HolidayDate >= start && h.HolidayDate <= end)
            .Select(h => h.HolidayDate)
            .ToListAsync();

        for (DateTime date = start; date <= end; date = date.AddDays(1))
        {
            //then the days which arent weekends are incremented 
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !publicHolidays.Contains(date))
            {
                workingDays++;
            }
        }

        return workingDays;
    }
}
