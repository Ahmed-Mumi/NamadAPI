﻿using System;

namespace NomadAPI.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderFullName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string COntent { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
