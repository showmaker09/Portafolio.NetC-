using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TherapyApp.Core.DTOs;

namespace TherapyApp.Core.Interfaces
{
    public interface ISessionService
    {
        Task<ApiResponse<SessionResponse>> OpenSessionAsync(SessionRequest request); // operacion asincrona para abrir una nueva sesión/ devuelve un ApiResponse con el SessionResponse
        Task<ApiResponse<SessionResponse>> CloseSessionAsync(Guid sessionId);
        Task<ApiResponse<SessionResponse>> GetActiveSessionAsync(Guid patientId);
    }
}
