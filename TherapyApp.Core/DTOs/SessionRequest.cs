using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TherapyApp.Core.DTOs
{
    public  class SessionRequest
    {
        public Guid PatientId { get; set; }
        public Guid TherapistId { get; set; }

    }


}
