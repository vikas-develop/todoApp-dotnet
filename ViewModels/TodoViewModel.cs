using System;
using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.ViewModels;

public partial class TodoViewModel : ViewModelBase
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private DateTime createdAt;

    [ObservableProperty]
    private DateTime? completedAt;

    [ObservableProperty]
    private bool isSelected;

    public TodoViewModel() { }

    public TodoViewModel(Todo todo)
    {
        Id = todo.Id;
        Title = todo.Title;
        Description = todo.Description;
        IsCompleted = todo.IsCompleted;
        CreatedAt = todo.CreatedAt;
        CompletedAt = todo.CompletedAt;
    }

    public Todo ToTodo()
    {
        return new Todo
        {
            Id = Id,
            Title = Title,
            Description = Description,
            IsCompleted = IsCompleted,
            CreatedAt = CreatedAt,
            CompletedAt = CompletedAt
        };
    }

    public string StatusText => IsCompleted ? "COMPLETED" : "PENDING";
    public string CreatedAtText => CreatedAt.ToString("MMM dd, yyyy HH:mm");
    public string? CompletedAtText => CompletedAt?.ToString("MMM dd, yyyy HH:mm");
}

