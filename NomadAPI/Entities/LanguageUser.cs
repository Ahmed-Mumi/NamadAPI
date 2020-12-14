namespace NomadAPI.Entities
{
    public class LanguageUser
    {
        public int Id { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public LanguageUserStatus LanguageUserStatus { get; set; }
        public int LanguageUserStatusId { get; set; }
    }
}
