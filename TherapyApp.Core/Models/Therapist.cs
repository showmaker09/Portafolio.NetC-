using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace TherapyApp.Core.Models
{
    public class Therapist
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegación
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();

    }
}
