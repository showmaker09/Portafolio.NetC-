using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.Models
{
   public class Session
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "open";         // "open", "closed"
        public DateTime? ClosedAt { get; set; }

        // Navegación
        public Patient Patient { get; set; } = null!;
        public Therapist Therapist { get; set; } = null!;
        public Report? Report { get; set; }

    }
}
