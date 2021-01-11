namespace NomadAPI.Dtos
{
    public class ReactionDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public bool PositiveReaction { get; set; }
    }
}
