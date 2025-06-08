using NotificationService.API.Models;
using NotificationService.API.Data;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationServiceDbContext _context;

        public NotificationService(NotificationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            notification.ReadAt = null;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(int id, Notification notification)
        {
            var existingNotification = await _context.Notifications.FindAsync(id);
            if (existingNotification == null)
                return null;

            existingNotification.Title = notification.Title;
            existingNotification.Message = notification.Message;
            existingNotification.Type = notification.Type;
            existingNotification.IsRead = notification.IsRead;
            existingNotification.UpdatedAt = DateTime.UtcNow;
            
            if (notification.IsRead && !existingNotification.IsRead)
            {
                existingNotification.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return existingNotification;
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkNotificationAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                notification.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            var now = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = now;
                notification.UpdatedAt = now;
            }

            await _context.SaveChangesAsync();
        }
    }
} 