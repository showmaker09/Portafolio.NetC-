using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid TherapistId { get; set; }
        public Guid ReportId { get; set; }
        public string Channel { get; set; } = "polling";     // "polling", "fcm"
        public string Status { get; set; } = "pending";      // "pending", "sent", "read"
        public DateTime? SentAt { get; set; }

        // Navegación
        public Therapist Therapist { get; set; } = null!;
        public Report Report { get; set; } = null!;

    }
}
