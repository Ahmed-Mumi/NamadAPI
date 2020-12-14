using System;

namespace NomadAPI.Dtos
{
    public class CreateTravelDto
    {
        public DateTime? TravelFromDate { get; set; }
        public DateTime? TravelToDate { get; set; }
        public bool Flexible { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
    }
}
