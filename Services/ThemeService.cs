using System;
using System.IO;
using Avalonia.Styling;

namespace TodoApp.Desktop.Services;

public class ThemeService
{
    private const string ThemeConfigFile = "theme.config";
    private ThemeVariant _currentTheme = ThemeVariant.Default;

    public ThemeVariant CurrentTheme => _currentTheme;

    public event EventHandler<ThemeVariant>? ThemeChanged;

    public ThemeService()
    {
        LoadTheme();
    }

    public void ToggleTheme()
    {
        _currentTheme = _currentTheme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        SaveTheme();
        ThemeChanged?.Invoke(this, _currentTheme);
    }

    public void SetTheme(ThemeVariant theme)
    {
        if (_currentTheme != theme)
        {
            _currentTheme = theme;
            SaveTheme();
            ThemeChanged?.Invoke(this, _currentTheme);
        }
    }

    private void LoadTheme()
    {
        try
        {
            if (File.Exists(ThemeConfigFile))
            {
                var themeString = File.ReadAllText(ThemeConfigFile).Trim();
                _currentTheme = themeString == "Dark" ? ThemeVariant.Dark : ThemeVariant.Light;
            }
        }
        catch
        {
            // Use default theme if loading fails
            _currentTheme = ThemeVariant.Light;
        }
    }

    private void SaveTheme()
    {
        try
        {
            File.WriteAllText(ThemeConfigFile, _currentTheme == ThemeVariant.Dark ? "Dark" : "Light");
        }
        catch
        {
            // Ignore save errors
        }
    }
}

