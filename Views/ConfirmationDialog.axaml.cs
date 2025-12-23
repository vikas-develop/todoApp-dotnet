using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TodoApp.Desktop.Views;

public partial class ConfirmationDialog : Window
{
    public string DialogTitle { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool Result { get; private set; }

    public ConfirmationDialog()
    {
        InitializeComponent();
        TitleText.Text = DialogTitle;
        MessageText.Text = Message;
    }

    public ConfirmationDialog(string title, string message) : this()
    {
        DialogTitle = title;
        Message = message;
        TitleText.Text = title;
        MessageText.Text = message;
    }

    private void Confirm_Click(object? sender, RoutedEventArgs e)
    {
        Result = true;
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Result = false;
        Close();
    }
}

