using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<TherapyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "TherapyApp API corriendo");
// app.MapPost

app.Run();