using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.Models
{
    public  class Report
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string StoragePath { get; set; } = string.Empty;
        public string StorageType { get; set; } = "local";   // "local", "drive"
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "pending";      // "pending", "generated", "sent"

        // Navegación
        public Session Session { get; set; } = null!;
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();




    }
}
