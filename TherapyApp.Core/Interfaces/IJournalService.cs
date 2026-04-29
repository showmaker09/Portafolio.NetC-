using TherapyApp.Core.DTOs;

namespace TherapyApp.Core.Interfaces
{

    public interface IJournalService
    {
        Task<ApiResponse<JournalEntryResponse>> CreateEntryAsync(JournalEntryRequest request); // journaEntryResponse es parte de la carpeta DTOS
        Task<ApiResponse<List<JournalEntryResponse>>> GetEntriesByPatientAsync(Guid patientId); // el funcionamiento de este metodo es obtener las entradas del paciente, por eso se le pasa el patientId
    }

}