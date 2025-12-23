using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Converters;

public class SortOptionToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SortOption sort)
        {
            return sort switch
            {
                SortOption.DateDescending => "Newest First",
                SortOption.DateAscending => "Oldest First",
                SortOption.TitleAscending => "Title (A-Z)",
                SortOption.TitleDescending => "Title (Z-A)",
                SortOption.StatusAscending => "Status (Pending First)",
                SortOption.StatusDescending => "Status (Completed First)",
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
                "Newest First" => SortOption.DateDescending,
                "Oldest First" => SortOption.DateAscending,
                "Title (A-Z)" => SortOption.TitleAscending,
                "Title (Z-A)" => SortOption.TitleDescending,
                "Status (Pending First)" => SortOption.StatusAscending,
                "Status (Completed First)" => SortOption.StatusDescending,
                _ => SortOption.DateDescending
            };
        }
        return SortOption.DateDescending;
    }
}

