using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Converters;

public class FilterStatusToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FilterStatus status)
        {
            return status switch
            {
                FilterStatus.All => "All",
                FilterStatus.Pending => "Pending",
                FilterStatus.Completed => "Completed",
                _ => value.ToString()
            };
        }
        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str switch
            {
                "All" => FilterStatus.All,
                "Pending" => FilterStatus.Pending,
                "Completed" => FilterStatus.Completed,
                _ => FilterStatus.All
            };
        }
        return FilterStatus.All;
    }
}

