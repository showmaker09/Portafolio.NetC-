using Microsoft.EntityFrameworkCore;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Interfaces;

namespace TherapyApp.Api.Endpoints;

public static class JournalEndpoints
{
    public static void MapJournalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/entries");

        group.MapPost("/", async (
            JournalEntryRequest request,
            IJournalService service) =>
        {
            var result = await service.CreateEntryAsync(request);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        });

        group.MapGet("/{patientId:guid}", async (
            Guid patientId,
            IJournalService service) =>
        {
            var result = await service.GetEntriesByPatientAsync(patientId);
            return Results.Ok(result);
        });
    }
}