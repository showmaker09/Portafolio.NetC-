using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
 
     public  class SessionRequest // esta clase se utiliza para abrir una nueva sesión, contiene el id del paciente y el id del terapeuta que se asignará a la sesión
    {
        public Guid PatientId { get; set; } // id de paciente
        public Guid TherapistId { get; set; } // id de terapeuta

    }


}
