using TherapyApp.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TherapyApp.Core.Interfaces
{

    public interface IJournalService
    {

        // Solo declara la firma del método. Nota que termina en un punto y coma (;), no tiene llaves { }.

        Task<ApiResponse<JournalEntryResponse>>CreateEntryAsync(JournalEntryRequest request); // journaEntryResponse es parte de la carpeta DTOS
        Task<ApiResponse<List<JournalEntryResponse>>> GetEntriesByPatientAsync(Guid patientId); // el funcionamiento de este metodo es obtener las entradas del paciente, por eso se le pasa el patientId
    }

}