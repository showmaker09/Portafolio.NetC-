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
        public DateTime StartDate { get; set; } // fecha de inicio de la sesión
        public DateTime? EndDate { get; set; } // fecha de fin de la sesión, puede ser null si la sesión aún está en curso
        public string Status { get; set; } = string.Empty; // estado de la sesión (por ejemplo, "En curso", "Finalizada", "Cancelada")
        public int TotalEntries { get; set; } // número total de entradas asociadas a esta sesión

    }
}
