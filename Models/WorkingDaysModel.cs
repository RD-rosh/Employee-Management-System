public class WorkingDaysModel
{
    //date range of 2 weeks
    public DateTime StartDate{ get; set; } = DateTime.Today.AddDays(-14);
    public DateTime EndDate{ get; set; } = DateTime.Today;       
    public int WorkingDays{ get; set; }
}