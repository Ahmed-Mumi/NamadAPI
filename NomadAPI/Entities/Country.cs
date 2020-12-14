using System.Collections.Generic;

namespace NomadAPI.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Flag { get; set; }
        public Continent Continent { get; set; }
        public int ContinentId { get; set; }
        public ICollection<CountryUser> Users { get; set; }

    }
}
