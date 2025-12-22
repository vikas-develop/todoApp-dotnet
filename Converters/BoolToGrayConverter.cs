using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TodoApp.Desktop.Converters;

public class BoolToGrayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted && isCompleted)
        {
            return new SolidColorBrush(Color.Parse("#64748B"));
        }
        return new SolidColorBrush(Color.Parse("#1E293B"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

