namespace NomadAPI.Entities
{
    public class UserReaction
    {
        public AppUser ReactedUser { get; set; }
        public int ReactedUserId { get; set; }
        public AppUser ReactedByUser { get; set; }
        public int ReactedByUserId { get; set; }
        public bool PositiveReaction { get; set; }
    }
}
