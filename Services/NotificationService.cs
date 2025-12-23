using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Desktop.Services;

public class NotificationMessage : ObservableObject
{
    private string _message = string.Empty;
    private NotificationType _type;
    private DateTime _timestamp;

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public NotificationType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set => SetProperty(ref _timestamp, value);
    }
}

public enum NotificationType
{
    Success,
    Error,
    Warning,
    Info
}

public class NotificationService : ObservableObject
{
    private readonly ObservableCollection<NotificationMessage> _notifications = new();
    public ObservableCollection<NotificationMessage> Notifications => _notifications;

    public void ShowSuccess(string message)
    {
        AddNotification(message, NotificationType.Success);
    }

    public void ShowError(string message)
    {
        AddNotification(message, NotificationType.Error);
    }

    public void ShowWarning(string message)
    {
        AddNotification(message, NotificationType.Warning);
    }

    public void ShowInfo(string message)
    {
        AddNotification(message, NotificationType.Info);
    }

    private void AddNotification(string message, NotificationType type)
    {
        var notification = new NotificationMessage
        {
            Message = message,
            Type = type,
            Timestamp = DateTime.Now
        };

        _notifications.Insert(0, notification);

        // Auto-remove after 5 seconds
        System.Threading.Tasks.Task.Delay(5000).ContinueWith(_ =>
        {
            _notifications.Remove(notification);
        });
    }

    public void RemoveNotification(NotificationMessage notification)
    {
        _notifications.Remove(notification);
    }
}

