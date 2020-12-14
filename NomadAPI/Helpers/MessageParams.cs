namespace NomadAPI.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
    }
}
