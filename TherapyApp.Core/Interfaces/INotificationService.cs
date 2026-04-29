using TherapyApp.Core.DTOs;

namespace TherapyApp.Core.Interfaces
{
    public interface INotificationService
    {
      Task NotifyTherapistAsync(Guid therapistId, Guid reportId); // este metodo se encarga de enviar una notificación al terapeuta, por eso se le pasa el therapistId y el reportId
      Task MarkNotificationReadAsync(Guid notificationId); // este metodo se encarga de marcar una notificación como leída, por eso se le pasa el notificationId
    }
}