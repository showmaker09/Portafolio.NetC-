using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;

using TherapyApp.Core.Interfaces;


namespace TherapyApp.Api.Endpoints
{


    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/reports");

            group.MapGet("/{therapistId:guid}", async (
                Guid therapistId,
                IReportService service) =>
            {
                var result = await service.GetReportsByTherapistAsync(therapistId);
                return Results.Ok(result);
            });

            group.MapGet("/pending/{therapistId:guid}", async (
                Guid therapistId,
                IReportService service) =>
            {
                var result = await service.GetPendingNotificationAsync(therapistId);
                return Results.Ok(result);
            });
        }
    }

}