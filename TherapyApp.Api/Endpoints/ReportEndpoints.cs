using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;

namespace TherapyApp.Api.Endpoints
{

    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/reports");

            // GET /api/reports/{therapistId} — terapeuta consulta sus reportes
            group.MapGet("/{therapistId:guid}", async (
                Guid therapistId,
                TherapyDbContext db) =>
            {
                var reports = await db.Reports
                    .Include(r => r.Session)
                        .ThenInclude(s => s.Patient)
                    .Where(r => r.Session.TherapistId == therapistId)
                    .OrderByDescending(r => r.GeneratedAt)
                    .Select(r => new ReportResponse
                    {
                        Id = r.Id,
                        SessionId = r.SessionId,
                        StoragePath = r.StoragePath,
                        Status = r.Status,
                        GeneratedAt = r.GeneratedAt,
                        PatientName = r.Session.Patient.FullName
                    })
                    .ToListAsync();

                return Results.Ok(ApiResponse<List<ReportResponse>>.Ok(reports));
            });

            // GET /api/reports/pending/{therapistId} — notificaciones pendientes
            group.MapGet("/pending/{therapistId:guid}", async (
                Guid therapistId,
                TherapyDbContext db) =>
            {
                var pending = await db.Notifications
                    .Include(n => n.Report)
                        .ThenInclude(r => r.Session)
                            .ThenInclude(s => s.Patient)
                    .Where(n => n.TherapistId == therapistId
                             && n.Status == "pending")
                    .Select(n => new ReportResponse
                    {
                        Id = n.Report.Id,
                        SessionId = n.Report.SessionId,
                        StoragePath = n.Report.StoragePath,
                        Status = n.Report.Status,
                        GeneratedAt = n.Report.GeneratedAt,
                        PatientName = n.Report.Session.Patient.FullName
                    })
                    .ToListAsync();

                return Results.Ok(ApiResponse<List<ReportResponse>>.Ok(pending));
            });
        }
    }

}