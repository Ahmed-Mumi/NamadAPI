namespace NomadAPI.Entities
{
    public class TravelCity
    {
        public Travel Travel { get; set; }
        public int TravelId { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
