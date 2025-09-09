using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createDto);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<bool> MarkAllNotificationsAsReadAsync(int userId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<NotificationStatsDto> GetNotificationStatsAsync(int userId);
        Task SendVisitaProximaNotificationAsync(int visitaId);
        Task SendVisitaExpiradaNotificationAsync(int visitaId);
        Task SendCheckInNotificationAsync(int visitaId);
        Task SendCheckOutNotificationAsync(int visitaId);
        Task SendColaboradorBlackListNotificationAsync(int colaboradorId);
        Task SendSistemaNotificationAsync(string titulo, string mensaje, PrioridadNotificacion prioridad);
        Task ProcessExpiredNotificationsAsync();
        Task<NotificationSettingsDto> GetNotificationSettingsAsync();
        Task<NotificationSettingsDto> UpdateNotificationSettingsAsync(NotificationSettingsDto settings);
    }
}
