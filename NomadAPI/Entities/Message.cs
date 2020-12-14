using System;

namespace NomadAPI.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string SenderFullName { get; set; }
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }
        public string RecipientFullName { get; set; }
        public string COntent { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    }
}
