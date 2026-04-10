using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
    public class SessionResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalEntries { get; set; }

    }
}
