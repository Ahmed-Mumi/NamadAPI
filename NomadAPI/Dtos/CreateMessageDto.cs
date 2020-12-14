namespace NomadAPI.Dtos
{
    public class CreateMessageDto
    {
        public int RecipientId { get; set; }
        public string RecipientEmail { get; set; }
        public string Content { get; set; }
    }
}
