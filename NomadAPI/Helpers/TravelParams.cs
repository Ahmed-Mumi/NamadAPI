using System;

namespace NomadAPI.Helpers
{
    public class TravelParams : PaginationParams
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public DateTime? TravelFromDate { get; set; }
        public DateTime? TravelToDate { get; set; }
    }
}
