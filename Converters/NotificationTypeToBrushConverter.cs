using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.Converters;

public class NotificationTypeToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is NotificationType type)
        {
            return type switch
            {
                NotificationType.Success => new SolidColorBrush(Color.Parse("#10B981")),
                NotificationType.Error => new SolidColorBrush(Color.Parse("#EF4444")),
                NotificationType.Warning => new SolidColorBrush(Color.Parse("#F59E0B")),
                NotificationType.Info => new SolidColorBrush(Color.Parse("#6366F1")),
                _ => new SolidColorBrush(Color.Parse("#64748B"))
            };
        }
        return new SolidColorBrush(Color.Parse("#64748B"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

