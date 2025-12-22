using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TodoApp.Desktop.Converters;

public class BoolToStatusBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            // Completed: Green, Pending: Blue/Indigo (more modern)
            return isCompleted ? new SolidColorBrush(Color.Parse("#10B981")) : new SolidColorBrush(Color.Parse("#6366F1"));
        }
        return new SolidColorBrush(Color.Parse("#64748B"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

