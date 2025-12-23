using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.Converters;

public class NotificationTypeToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is NotificationType type)
        {
            return type switch
            {
                NotificationType.Success => "✓",
                NotificationType.Error => "✕",
                NotificationType.Warning => "⚠",
                NotificationType.Info => "ℹ",
                _ => "•"
            };
        }
        return "•";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

