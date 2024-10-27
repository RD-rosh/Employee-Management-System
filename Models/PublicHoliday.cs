using System;
using System.Collections.Generic;

namespace Employee_Management_System.Models;

public partial class PublicHoliday
{
    public int Id { get; set; }

    public string HolidayName { get; set; } = null!;

    public DateTime HolidayDate { get; set; }
}
