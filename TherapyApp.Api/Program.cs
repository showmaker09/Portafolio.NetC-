using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Api.Middleware;
using TherapyApp.Api.Endpoints; 
var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<TherapyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();


app.MapGet("/", () => "TherapyApp API corriendo");
// app.MapPost
app.MapJournalEndpoints();
app.MapSessionEndpoints();
app.MapReportEndpoints();

app.Run();