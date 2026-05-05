using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Interfaces;

namespace TherapyApp.Api.Services;

public class ReportService : IReportService
{
    private readonly TherapyDbContext _db;

    public ReportService(TherapyDbContext db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<ReportResponse>>> GetReportsByTherapistAsync(
        Guid therapistId)
    {
        var reports = await _db.Reports
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

        return ApiResponse<List<ReportResponse>>.Ok(reports);
    }

    public async Task<ApiResponse<List<ReportResponse>>> GetPendingNotificationAsync(
        Guid therapistId)
    {
        var pending = await _db.Notifications
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

        return ApiResponse<List<ReportResponse>>.Ok(pending);
    }
}