using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TodoCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Don't select if clicking on a button
        if (e.Source is Button)
            return;

        if (sender is Border border && border.Tag is TodoViewModel todo && DataContext is MainWindowViewModel vm)
        {
            vm.SelectTodoCommand.Execute(todo);
        }
    }

    private void ToggleComplete_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TodoViewModel todo && DataContext is MainWindowViewModel vm)
        {
            vm.ToggleCompleteCommand.Execute(todo);
            e.Handled = true; // Prevent card selection
        }
    }

    private void EditTodo_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TodoViewModel todo && DataContext is MainWindowViewModel vm)
        {
            vm.EditTodoCommand.Execute(todo);
            e.Handled = true; // Prevent card selection
        }
    }

    private void DeleteTodo_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is TodoViewModel todo && DataContext is MainWindowViewModel vm)
        {
            vm.DeleteTodoCommand.Execute(todo);
            e.Handled = true; // Prevent card selection
        }
    }
}

