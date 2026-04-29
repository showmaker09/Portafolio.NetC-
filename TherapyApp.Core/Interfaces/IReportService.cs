using TherapyApp.Core.DTOs;

namespace TherapyApp.Core.Interfaces
{
    public interface IReportService 
    {
        
        Task <ApiResponse<List<ReportResponse>>> GetReportsByTherapistAsync(Guid therapistId); // este metodo se encarga de obtener los reportes de un terapeuta específico, por eso se le pasa el therapistId(tipo lista)
        Task <ApiResponse<List<ReportResponse>>>GetPendingNotificationAsync(Guid therapistId); // este metodo se encarga de obtener un reporte específico de una sesión, por eso se le pasa el sessionId
    }


}