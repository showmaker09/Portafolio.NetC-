using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Api.Endpoints; 
using TherapyApp.Api.Middleware;
using TherapyApp.Api.Services;
using TherapyApp.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<TherapyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();


app.MapGet("/", () => "TherapyApp API corriendo");
// app.MapPost
app.MapJournalEndpoints();
app.MapSessionEndpoints();
app.MapReportEndpoints();

app.Run();