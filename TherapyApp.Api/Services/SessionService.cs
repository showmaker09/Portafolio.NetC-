using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Interfaces;
using TherapyApp.Core.Models;

namespace TherapyApp.Api.Services;

public class SessionService : ISessionService
{
    private readonly TherapyDbContext _db;
    private readonly INotificationService _notifications;

    public SessionService(TherapyDbContext db, INotificationService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    public async Task<ApiResponse<SessionResponse>> OpenSessionAsync(
        SessionRequest request)
    {
        var openExists = await _db.Sessions
            .AnyAsync(s => s.PatientId == request.PatientId
                        && s.Status == "open");

        if (openExists)
            return ApiResponse<SessionResponse>.Fail(
                "El paciente ya tiene una sesión abierta.");

        var session = new Session
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            TherapistId = request.TherapistId,
            StartDate = DateTime.UtcNow,
            Status = "open"
        };

        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();

        return ApiResponse<SessionResponse>.Ok(new SessionResponse
        {
            Id = session.Id,
            PatientId = session.PatientId,
            TherapistId = session.TherapistId,
            StartDate = session.StartDate,
            Status = session.Status,
            TotalEntries = 0
        }, "Sesión abierta correctamente.");
    }

    public async Task<ApiResponse<SessionResponse>> CloseSessionAsync(Guid sessionId)
    {
        var session = await _db.Sessions
            .Include(s => s.Patient)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session is null)
            return ApiResponse<SessionResponse>.Fail("Sesión no encontrada.");

        if (session.Status == "closed")
            return ApiResponse<SessionResponse>.Fail("La sesión ya está cerrada.");

        var totalEntries = await _db.JournalEntries
            .CountAsync(e => e.PatientId == session.PatientId
                          && e.RecordedAt >= session.StartDate);

        session.Status = "closed";
        session.EndDate = DateTime.UtcNow;
        session.ClosedAt = DateTime.UtcNow;

        var report = new Report
        {
            Id = Guid.NewGuid(),
            SessionId = session.Id,
            StoragePath = $"reports/{session.Id}.pdf",
            StorageType = "local",
            Status = "pending",
            GeneratedAt = DateTime.UtcNow
        };

        _db.Reports.Add(report);
        await _db.SaveChangesAsync();

        // Notificar al terapeuta
        await _notifications.NotifyTherapistAsync(session.TherapistId, report.Id);

        return ApiResponse<SessionResponse>.Ok(new SessionResponse
        {
            Id = session.Id,
            PatientId = session.PatientId,
            TherapistId = session.TherapistId,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            Status = session.Status,
            TotalEntries = totalEntries
        }, $"Sesión cerrada. {totalEntries} entradas registradas.");
    }

    public async Task<ApiResponse<SessionResponse>> GetActiveSessionAsync(Guid patientId)
    {
        var session = await _db.Sessions
            .Where(s => s.PatientId == patientId && s.Status == "open")
            .Select(s => new SessionResponse
            {
                Id = s.Id,
                PatientId = s.PatientId,
                TherapistId = s.TherapistId,
                StartDate = s.StartDate,
                Status = s.Status,
                TotalEntries = _db.JournalEntries
                    .Count(e => e.PatientId == patientId
                             && e.RecordedAt >= s.StartDate)
            })
            .FirstOrDefaultAsync();

        if (session is null)
            return ApiResponse<SessionResponse>.Fail(
                "No hay sesión activa para este paciente.");

        return ApiResponse<SessionResponse>.Ok(session);
    }
}