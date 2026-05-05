using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.DTOs;
using TherapyApp.Core.Interfaces;
using TherapyApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;


namespace TherapyApp.Api.Services 
{

public class JournalService : IJournalService  //   implementación de una interfaz.
 {
    private readonly TherapyDbContext _db;

    public JournalService(TherapyDbContext db) // CONSTRUCTOR PARA INYECTAR EL CONTEXTO DE LA BASE DE DATOS
    {
        _db = db;
    }


        // esta funcion  realiza:Aquí SÍ se implementa la función (el cómo). Se abren llaves { } y se escribe la lógica real.

        public async Task<ApiResponse<JournalEntryResponse>> CreateEntryAsync(JournalEntryRequest request)
        {
        if (string.IsNullOrWhiteSpace(request.Content))
            return ApiResponse<JournalEntryResponse>.Fail(
                "El contenido no puede estar vacío.");

        if (request.MoodScore < 1 || request.MoodScore > 10)
            return ApiResponse<JournalEntryResponse>.Fail(
                "MoodScore debe estar entre 1 y 10.");

        var patient = await _db.Patients
            .FirstOrDefaultAsync(p => p.Id == request.PatientId);

        if (patient is null)
            return ApiResponse<JournalEntryResponse>.Fail(
                "Paciente no encontrado.");

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

        _db.JournalEntries.Add(entry);
        await _db.SaveChangesAsync();

        return ApiResponse<JournalEntryResponse>.Ok(new JournalEntryResponse
        {
            Id = entry.Id,
            PatientId = entry.PatientId,
            EntryType = entry.EntryType,
            Content = entry.Content,
            MoodScore = entry.MoodScore,
            RecordedAt = entry.RecordedAt,
            IsSynced = entry.IsSynced
        }, "Entrada registrada correctamente.");
    }

    public async Task<ApiResponse<List<JournalEntryResponse>>> GetEntriesByPatientAsync(
        Guid patientId)
    {
        var entries = await _db.JournalEntries
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

        return ApiResponse<List<JournalEntryResponse>>.Ok(entries);
    }
  } 
}
