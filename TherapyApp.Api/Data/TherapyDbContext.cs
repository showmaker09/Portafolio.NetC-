using Microsoft.EntityFrameworkCore;
using TherapyApp.Core.Models;

namespace TherapyApp.Api.Data;

public class TherapyDbContext : DbContext
{
    public TherapyDbContext(DbContextOptions<TherapyDbContext> options)
        : base(options) { }

    // Una DbSet por cada entidad = una tabla en SQL Server
    public DbSet<Therapist> Therapists => Set<Therapist>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Notification> Notifications => Set<Notification>();
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Therapist
        modelBuilder.Entity<Therapist>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.FullName).IsRequired().HasMaxLength(150);
            e.Property(t => t.Email).IsRequired().HasMaxLength(200);
            e.Property(t => t.ApiKey).IsRequired().HasMaxLength(100);
            e.HasIndex(t => t.ApiKey).IsUnique();
            e.HasIndex(t => t.Email).IsUnique();
        });

        // Patient
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.FullName).IsRequired().HasMaxLength(150);
            e.Property(p => p.Email).IsRequired().HasMaxLength(200);
            e.Property(p => p.ApiKey).IsRequired().HasMaxLength(100);
            e.HasIndex(p => p.ApiKey).IsUnique();
            e.HasOne(p => p.Therapist)
             .WithMany(t => t.Patients)
             .HasForeignKey(p => p.TherapistId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // JournalEntry
        modelBuilder.Entity<JournalEntry>(e =>
        {
            e.HasKey(j => j.Id);
            e.Property(j => j.Content).IsRequired().HasMaxLength(5000);
            e.Property(j => j.EntryType).IsRequired().HasMaxLength(50);
            e.HasOne(j => j.Patient)
             .WithMany(p => p.JournalEntries)
             .HasForeignKey(j => j.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Session
        modelBuilder.Entity<Session>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Status).IsRequired().HasMaxLength(20);
            e.HasOne(s => s.Patient)
             .WithMany(p => p.Sessions)
             .HasForeignKey(s => s.PatientId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Therapist)
             .WithMany(t => t.Sessions)
             .HasForeignKey(s => s.TherapistId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Report
        modelBuilder.Entity<Report>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.StoragePath).IsRequired().HasMaxLength(500);
            e.Property(r => r.StorageType).HasMaxLength(20);
            e.Property(r => r.Status).HasMaxLength(20);
            e.HasOne(r => r.Session)
             .WithOne(s => s.Report)
             .HasForeignKey<Report>(r => r.SessionId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Notification
        modelBuilder.Entity<Notification>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Channel).HasMaxLength(20);
            e.Property(n => n.Status).HasMaxLength(20);
            e.HasOne(n => n.Therapist)
             .WithMany()
             .HasForeignKey(n => n.TherapistId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(n => n.Report)
             .WithMany(r => r.Notifications)
             .HasForeignKey(n => n.ReportId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}