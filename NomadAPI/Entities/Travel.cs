using System;
using System.Collections.Generic;

namespace NomadAPI.Entities
{
    public class Travel
    {
        public int Id { get; set; }
        public DateTime? TravelFromDate { get; set; }
        public DateTime? TravelToDate { get; set; }
        public DateTime PostedDate { get; set; }
        public bool Flexible { get; set; } = false;
        public string Description { get; set; }
        public bool Active { get; set; } = true;
        public int NumberOfApplicants { get; set; } = 0;
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public MeansOfTravel MeansOfTravel { get; set; }
        public int? MeansOfTravelId { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<TravelCity> TravelCities { get; set; }

    }
}
