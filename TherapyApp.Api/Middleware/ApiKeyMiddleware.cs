using TherapyApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TherapyApp.Api.Middleware;

public class ApiKeyMiddleware
{   // Request Delegate  su función es recibir el contexto de la petición, procesarlo y luego pasar al siguiente middleware en la cadena.
    private readonly RequestDelegate _next;
    private const string ApiKeyHeader = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TherapyDbContext db)
    {
        // Permitir el endpoint raíz sin key (es solo el ping)
        if (context.Request.Path == "/")
        {
            await _next(context);
            return;
        }

        // Verificar que el header existe 
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var keyValue))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key requerida.");
            return;
        }

        var apiKey = keyValue.ToString();

        // Buscar en terapeutas
        var isTherapist = await db.Therapists
            .AnyAsync(t => t.ApiKey == apiKey);

        // Buscar en pacientes
        var isPatient = await db.Patients
            .AnyAsync(p => p.ApiKey == apiKey && p.IsActive);

        if (!isTherapist && !isPatient)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        // Guardar en el contexto quién está llamando
        // Útil para los endpoints — saben si es terapeuta o paciente
        context.Items["IsTherapist"] = isTherapist;
        context.Items["IsPatient"] = isPatient;
        context.Items["ApiKey"] = apiKey;

        await _next(context);
    }
}