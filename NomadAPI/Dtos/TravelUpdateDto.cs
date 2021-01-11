using NomadAPI.Entities;
using System;
using System.Collections.Generic;

namespace NomadAPI.Dtos
{
    public class TravelUpdateDto
    {
        public int Id { get; set; }
        public DateTime? TravelFromDate { get; set; }
        public DateTime? TravelToDate { get; set; }
        public bool Flexible { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public int? MeansOfTravelId { get; set; }


        //remove
        public ICollection<TravelCity> TravelCities { get; set; }
    }
}
