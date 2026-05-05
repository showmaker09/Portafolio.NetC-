using TherapyApp.Core.DTOs;
using TherapyApp.Core.Interfaces;

namespace TherapyApp.Api.Endpoints
{

    public static class SessionEndpoints
    {
        public static void MapSessionEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/sessions");

            group.MapPost("/", async (
                SessionRequest request,
                ISessionService service) =>
            {
                var result = await service.OpenSessionAsync(request);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPost("/{id:guid}/close", async (
                Guid id,
                ISessionService service) =>
            {
                var result = await service.CloseSessionAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            group.MapGet("/{patientId:guid}", async (
                Guid patientId,
                ISessionService service) =>
            {
                var result = await service.GetActiveSessionAsync(patientId);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });
        }
    }
}