using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using TodoApp.Desktop.Services;
using TodoApp.Desktop.ViewModels;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop;

public partial class App : Application
{
    private ThemeService? _themeService;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _themeService = new ThemeService();
            _themeService.ThemeChanged += (s, theme) =>
            {
                RequestedThemeVariant = theme;
            };
            
            // Apply saved theme
            RequestedThemeVariant = _themeService.CurrentTheme;
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_themeService),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

