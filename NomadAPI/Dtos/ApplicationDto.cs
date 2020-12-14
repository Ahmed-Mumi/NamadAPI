using System;

namespace NomadAPI.Dtos
{
    public class ApplicationDto
    {
        public int UserAppliedAdId { get; set; }
        public string UserAppliedFullName { get; set; }
        public int TravelId { get; set; }
        public DateTime AppliedDate { get; set; }
        public bool Official { get; set; }
    }
}
