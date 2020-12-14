namespace NomadAPI.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ToVisit { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
    }
}
