using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace TherapyApp.Core.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public Guid TherapistId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty; // Cuando la app MAUI hace una llamada a la API, manda esa clave en el header del request y le da acceso a los datos
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegación
        public Therapist Therapist { get; set; } = null!;
        public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();


    }
}
