using NotificationService.API.Models;

namespace NotificationService.API.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> UpdateNotificationAsync(int id, Notification notification);
        Task DeleteNotificationAsync(int id);
        Task MarkNotificationAsReadAsync(int id);
        Task MarkAllNotificationsAsReadAsync(int userId);
    }
} 