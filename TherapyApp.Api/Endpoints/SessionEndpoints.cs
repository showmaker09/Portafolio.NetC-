using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Models;

namespace TherapyApp.Api.Endpoints
{

    public static class SessionEndpoints
    {
        public static void MapSessionEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/sessions");

            // POST /api/sessions — abrir nueva sesión
            group.MapPost("/", async (
                SessionRequest request,
                TherapyDbContext db) =>
            {
                // Verificar que no hay sesión abierta para este paciente
                var openSession = await db.Sessions
                    .AnyAsync(s => s.PatientId == request.PatientId
                                && s.Status == "open");

                if (openSession)
                    return Results.BadRequest(
                        ApiResponse<string>.Fail("El paciente ya tiene una sesión abierta."));

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    PatientId = request.PatientId,
                    TherapistId = request.TherapistId,
                    StartDate = DateTime.UtcNow,
                    Status = "open"
                };

                db.Sessions.Add(session);
                await db.SaveChangesAsync();

                var response = new SessionResponse
                {
                    Id = session.Id,
                    PatientId = session.PatientId,
                    TherapistId = session.TherapistId,
                    StartDate = session.StartDate,
                    Status = session.Status,
                    TotalEntries = 0
                };

                return Results.Ok(ApiResponse<SessionResponse>.Ok(
                    response, "Sesión abierta correctamente."));
            });

            // POST /api/sessions/{id}/close — cerrar sesión y notificar
            group.MapPost("/{id:guid}/close", async (
                Guid id,    
                TherapyDbContext db) =>
            {
                var session = await db.Sessions
                    .Include(s => s.Patient)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (session is null)
                    return Results.NotFound(
                        ApiResponse<string>.Fail("Sesión no encontrada."));

                if (session.Status == "closed")
                    return Results.BadRequest(
                        ApiResponse<string>.Fail("La sesión ya está cerrada."));

                // Contar entradas del período
                var totalEntries = await db.JournalEntries
                    .CountAsync(e => e.PatientId == session.PatientId
                                  && e.RecordedAt >= session.StartDate);

                // Cerrar sesión
                session.Status = "closed";
                session.EndDate = DateTime.UtcNow;
                session.ClosedAt = DateTime.UtcNow;

                // Crear notificación para el terapeuta
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    TherapistId = session.TherapistId,
                    ReportId = Guid.Empty, // se actualiza cuando se genere el PDF
                    Channel = "polling",
                    Status = "pending",
                };

                // Crear reporte pendiente
                var report = new Report
                {
                    Id = Guid.NewGuid(),
                    SessionId = session.Id,
                    StoragePath = $"reports/{session.Id}.pdf",
                    StorageType = "local",
                    Status = "pending",
                    GeneratedAt = DateTime.UtcNow
                };

                db.Reports.Add(report);
                await db.SaveChangesAsync();

                // Actualizar notificación con el ReportId real
                notification.ReportId = report.Id;
                db.Notifications.Add(notification);
                await db.SaveChangesAsync();

                var response = new SessionResponse
                {
                    Id = session.Id,
                    PatientId = session.PatientId,
                    TherapistId = session.TherapistId,
                    StartDate = session.StartDate,
                    EndDate = session.EndDate,
                    Status = session.Status,
                    TotalEntries = totalEntries
                };

                return Results.Ok(ApiResponse<SessionResponse>.Ok(
                    response, $"Sesión cerrada. {totalEntries} entradas registradas."));
            });

            // GET /api/sessions/{patientId} — sesión activa del paciente
            group.MapGet("/{patientId:guid}", async (
                Guid patientId,
                TherapyDbContext db) =>
            {
                var session = await db.Sessions
                    .Where(s => s.PatientId == patientId && s.Status == "open")
                    .Select(s => new SessionResponse
                    {
                        Id = s.Id,
                        PatientId = s.PatientId,
                        TherapistId = s.TherapistId,
                        StartDate = s.StartDate,
                        Status = s.Status,
                        TotalEntries = db.JournalEntries
                            .Count(e => e.PatientId == patientId
                                     && e.RecordedAt >= s.StartDate)
                    })
                    .FirstOrDefaultAsync();

                if (session is null)
                    return Results.NotFound(
                        ApiResponse<string>.Fail("No hay sesión activa para este paciente."));

                return Results.Ok(ApiResponse<SessionResponse>.Ok(session));
            });
        }
    }
}