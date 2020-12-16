using System;

namespace NomadAPI.Entities
{
    public class Application
    {
        public AppUser UserAppliedAd { get; set; }
        public int UserAppliedAdId { get; set; }
        public int UserPostedAdId { get; set; }
        public Travel Travel { get; set; }
        public int TravelId { get; set; }
        public DateTime AppliedDate { get; set; }
        public bool Official { get; set; } = false;
    }
}
