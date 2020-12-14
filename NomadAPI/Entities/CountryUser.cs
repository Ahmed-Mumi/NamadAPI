namespace NomadAPI.Entities
{
    public class CountryUser
    {
        public int Id { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public CountryUserStatus CountryUserStatus { get; set; }
        public int CountryUserStatusId { get; set; }
    }
}
