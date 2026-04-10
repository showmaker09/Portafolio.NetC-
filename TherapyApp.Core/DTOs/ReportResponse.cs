using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
    public class ReportResponse
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string StoragePath { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string PatientName { get; set; } = string.Empty;

    }
}
