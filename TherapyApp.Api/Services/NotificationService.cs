using Microsoft.EntityFrameworkCore;
using TherapyApp.Api.Data;
using TherapyApp.Core.Interfaces;
using TherapyApp.Core.Models;

namespace TherapyApp.Api.Services;

public class NotificationService : INotificationService
{
    private readonly TherapyDbContext _db;

    public NotificationService(TherapyDbContext db)
    {
        _db = db;
    }

    public async Task NotifyTherapistAsync(Guid therapistId, Guid reportId)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            TherapistId = therapistId,
            ReportId = reportId,
            Channel = "polling",
            Status = "pending",
            SentAt = DateTime.UtcNow
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
    }

    public async Task MarkNotificationReadAsync(Guid notificationId)
    {
        var notification = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId);

        if (notification is null) return;

        notification.Status = "read";
        await _db.SaveChangesAsync();
    }
}