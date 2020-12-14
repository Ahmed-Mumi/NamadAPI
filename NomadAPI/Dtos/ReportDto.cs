using System;
using System.ComponentModel.DataAnnotations;

namespace NomadAPI.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public DateTime DateOfReport { get; set; } = DateTime.UtcNow;
        [Required]
        public string Text { get; set; }
        public int UserReportsId { get; set; }
        public string UserReportsFullName { get; set; }
        public int UserReportedId { get; set; }
        public string UserReportedFullName { get; set; }
    }
}
