namespace NomadAPI.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int LastMessageId { get; set; }

        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
