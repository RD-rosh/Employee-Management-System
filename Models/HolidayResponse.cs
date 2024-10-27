//deserialize response
public class HolidayResponse
    {
        public HolidayResponseData Response { get; set; }
    }

    public class HolidayResponseData
    {
        public List<HolidayData> Holidays { get; set; }
    }

    public class HolidayData
    {
        public string Name { get; set; }
        public HolidayDate Date { get; set; }
    }

    public class HolidayDate
    {
        public string Iso { get; set; }
    }
