using System;

namespace NomadAPI.Entities
{
    public class Friendship
    {
        public AppUser UserSentRequest { get; set; }
        public int UserSentRequestId { get; set; }
        public AppUser UserReceivedRequest { get; set; }
        public int UserReceivedRequestId { get; set; }
        public DateTime SentFriendshipDate { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedFriendshipDate { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }
        public int FriendshipStatusId { get; set; }
    }
}
