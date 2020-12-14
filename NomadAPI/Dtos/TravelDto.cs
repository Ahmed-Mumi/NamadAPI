using NomadAPI.Entities;
using System;
using System.Collections.Generic;

namespace NomadAPI.Dtos
{
    public class TravelDto
    {
        public int Id { get; set; }
        public DateTime? TravelFromDate { get; set; }
        public DateTime? TravelToDate { get; set; }
        public DateTime PostedDate { get; set; }
        public bool Flexible { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int NumberOfApplicants { get; set; }
        public string Url { get; set; }
        public ICollection<TravelCity> TravelCities { get; set; }
    }
}
