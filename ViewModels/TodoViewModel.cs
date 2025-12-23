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

    partial void OnTitleChanged(string value)
    {
        // Validate in real-time
        ValidateTitleProperty();
    }

    public void ValidateTitleProperty()
    {
        // Check length before trimming (to catch when user types too much)
        if (Title.Length > MaxTitleLength)
        {
            TitleError = $"Title must be {MaxTitleLength} characters or less";
            return;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            TitleError = "Title is required";
            return;
        }

        // If we get here, title is valid
        TitleError = string.Empty;
    }

    [ObservableProperty]
    private string description = string.Empty;

    partial void OnDescriptionChanged(string value)
    {
        // Validate in real-time
        ValidateDescriptionProperty();
    }

    public void ValidateDescriptionProperty()
    {
        if (!string.IsNullOrWhiteSpace(Description) && Description.Length > MaxDescriptionLength)
        {
            DescriptionError = $"Description must be {MaxDescriptionLength} characters or less";
            return;
        }

        DescriptionError = string.Empty;
    }

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private DateTime createdAt;

    [ObservableProperty]
    private DateTime? completedAt;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private int? categoryId;

    [ObservableProperty]
    private Category? category;

    [ObservableProperty]
    private PriorityLevel priority = PriorityLevel.Medium;

    private string _titleError = string.Empty;
    public string TitleError
    {
        get => _titleError;
        set => SetProperty(ref _titleError, value);
    }

    private string _descriptionError = string.Empty;
    public string DescriptionError
    {
        get => _descriptionError;
        set => SetProperty(ref _descriptionError, value);
    }

    private const int MaxTitleLength = 200;
    private const int MaxDescriptionLength = 1000;

    public TodoViewModel() { }

    public TodoViewModel(Todo todo)
    {
        Id = todo.Id;
        Title = todo.Title;
        Description = todo.Description;
        IsCompleted = todo.IsCompleted;
        CreatedAt = todo.CreatedAt;
        CompletedAt = todo.CompletedAt;
        CategoryId = todo.CategoryId;
        Category = todo.Category;
        Priority = todo.Priority;
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
            CompletedAt = CompletedAt,
            CategoryId = CategoryId,
            Priority = Priority
        };
    }

    public string CategoryName => Category?.Name ?? "No Category";
    public string CategoryColor => Category?.Color ?? "#94A3B8"; // Gray for no category

    public string PriorityText => Priority.ToString();
    public string PriorityColor => Priority switch
    {
        PriorityLevel.High => "#EF4444",   // Red
        PriorityLevel.Medium => "#F59E0B", // Amber
        PriorityLevel.Low => "#22C55E",    // Green
        _ => "#94A3B8"
    };

    public string StatusText => IsCompleted ? "COMPLETED" : "PENDING";
    public string CreatedAtText => CreatedAt.ToString("MMM dd, yyyy HH:mm");
    public string? CompletedAtText => CompletedAt?.ToString("MMM dd, yyyy HH:mm");

    public bool ValidateTitle(out string error)
    {
        error = string.Empty;
        
        if (string.IsNullOrWhiteSpace(Title))
        {
            error = "Title is required";
            return false;
        }

        var trimmedTitle = Title.Trim();
        if (trimmedTitle.Length > MaxTitleLength)
        {
            error = $"Title must be {MaxTitleLength} characters or less";
            return false;
        }

        return true;
    }

    public bool ValidateDescription(out string error)
    {
        error = string.Empty;
        
        if (!string.IsNullOrWhiteSpace(Description) && Description.Length > MaxDescriptionLength)
        {
            error = $"Description must be {MaxDescriptionLength} characters or less";
            return false;
        }

        return true;
    }

    public bool Validate(out string error)
    {
        if (!ValidateTitle(out var titleError))
        {
            error = titleError;
            TitleError = titleError;
            return false;
        }

        if (!ValidateDescription(out var descError))
        {
            error = descError;
            DescriptionError = descError;
            return false;
        }

        TitleError = string.Empty;
        DescriptionError = string.Empty;
        error = string.Empty;
        return true;
    }
}

