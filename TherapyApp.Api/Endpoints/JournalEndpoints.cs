using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Models;

namespace TherapyApp.Api.Endpoints
{

    public static class JournalEndpoints
    {
        public static void MapJournalEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/entries");

            // POST /api/entries — paciente registra una entrada
            group.MapPost("/", async (
                JournalEntryRequest request,
                TherapyDbContext db,
                HttpContext context) =>
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(request.Content))
                    return Results.BadRequest(
                        ApiResponse<string>.Fail("El contenido no puede estar vacío."));

                if (request.MoodScore < 1 || request.MoodScore > 10)
                    return Results.BadRequest(
                        ApiResponse<string>.Fail("MoodScore debe estar entre 1 y 10."));

                // Verificar que el paciente existe
                var patient = await db.Patients
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);

                if (patient is null)
                    return Results.NotFound(
                        ApiResponse<string>.Fail("Paciente no encontrado."));

                // Crear la entrada
                var entry = new JournalEntry
                {
                    Id = Guid.NewGuid(),
                    PatientId = request.PatientId,
                    EntryType = request.EntryType,
                    Content = request.Content,
                    MoodScore = request.MoodScore,
                    RecordedAt = DateTime.UtcNow,
                    IsSynced = true,
                    SyncedAt = DateTime.UtcNow
                };

                db.JournalEntries.Add(entry);
                await db.SaveChangesAsync();

                var response = new JournalEntryResponse
                {
                    Id = entry.Id,
                    PatientId = entry.PatientId,
                    EntryType = entry.EntryType,
                    Content = entry.Content,
                    MoodScore = entry.MoodScore,
                    RecordedAt = entry.RecordedAt,
                    IsSynced = entry.IsSynced
                };

                return Results.Ok(ApiResponse<JournalEntryResponse>.Ok(
                    response, "Entrada registrada correctamente."));
            });

            // GET /api/entries/{patientId} — consultar entradas de un paciente
            group.MapGet("/{patientId:guid}", async (
                Guid patientId,
                TherapyDbContext db) =>
            {
                var entries = await db.JournalEntries
                    .Where(e => e.PatientId == patientId)
                    .OrderByDescending(e => e.RecordedAt)
                    .Select(e => new JournalEntryResponse
                    {
                        Id = e.Id,
                        PatientId = e.PatientId,
                        EntryType = e.EntryType,
                        Content = e.Content,
                        MoodScore = e.MoodScore,
                        RecordedAt = e.RecordedAt,
                        IsSynced = e.IsSynced
                    })
                    .ToListAsync();

                return Results.Ok(ApiResponse<List<JournalEntryResponse>>.Ok(entries));
            });
        }
    }
}