using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime DateOfReport { get; set; }
        public string Text { get; set; }
        public AppUser UserReports { get; set; }
        public int UserReportsId { get; set; }
        public AppUser UserReported { get; set; }
        public int UserReportedId { get; set; }
    }
}
