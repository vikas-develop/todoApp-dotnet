using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Desktop.Models;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly TodoService _todoService;
    private readonly NotificationService _notificationService;
    private readonly ConfirmationService _confirmationService;
    private TodoViewModel? _selectedTodo;

    public MainWindowViewModel()
    {
        _todoService = new TodoService();
        _notificationService = new NotificationService();
        _confirmationService = new ConfirmationService();
        LoadTodos();
        
        AddTodoCommand = new RelayCommand(AddTodo);
        DeleteTodoCommand = new AsyncRelayCommand<TodoViewModel>(DeleteTodoAsync, CanDeleteTodo);
        ToggleCompleteCommand = new RelayCommand<TodoViewModel>(ToggleComplete);
        EditTodoCommand = new RelayCommand<TodoViewModel>(EditTodo);
        SelectTodoCommand = new RelayCommand<TodoViewModel>(SelectTodo);
        SaveTodoCommand = new RelayCommand(SaveTodo);
        CancelEditCommand = new RelayCommand(CancelEdit);
    }

    public NotificationService NotificationService => _notificationService;
    public ConfirmationService ConfirmationService => _confirmationService;

    public ObservableCollection<TodoViewModel> Todos { get; } = new();

    public TodoViewModel? SelectedTodo
    {
        get => _selectedTodo;
        set
        {
            // Clear previous selection
            if (_selectedTodo != null)
            {
                _selectedTodo.IsSelected = false;
            }

            SetProperty(ref _selectedTodo, value);

            // Set new selection
            if (_selectedTodo != null)
            {
                _selectedTodo.IsSelected = true;
            }

            OnPropertyChanged(nameof(IsTodoSelected));
            OnPropertyChanged(nameof(IsEditing));
        }
    }

    private string _newTodoTitle = string.Empty;
    private string _newTodoTitleError = string.Empty;
    
    public string NewTodoTitle
    {
        get => _newTodoTitle;
        set
        {
            SetProperty(ref _newTodoTitle, value);
            // Validate in real-time
            ValidateNewTodoTitle();
        }
    }

    public string NewTodoTitleError
    {
        get => _newTodoTitleError;
        set => SetProperty(ref _newTodoTitleError, value);
    }

    private void ValidateNewTodoTitle()
    {
        // Check length before trimming (to catch when user types too much)
        if (NewTodoTitle.Length > 200)
        {
            NewTodoTitleError = "Title must be 200 characters or less";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewTodoTitle))
        {
            NewTodoTitleError = "Title is required";
            return;
        }

        // If we get here, title is valid
        NewTodoTitleError = string.Empty;
    }

    private string _newTodoDescription = string.Empty;
    private string _newTodoDescriptionError = string.Empty;
    
    public string NewTodoDescription
    {
        get => _newTodoDescription;
        set
        {
            SetProperty(ref _newTodoDescription, value);
            // Validate in real-time
            ValidateNewTodoDescription();
        }
    }

    public string NewTodoDescriptionError
    {
        get => _newTodoDescriptionError;
        set => SetProperty(ref _newTodoDescriptionError, value);
    }

    private void ValidateNewTodoDescription()
    {
        // Check length (description is optional, but if provided, must be within limit)
        if (!string.IsNullOrWhiteSpace(NewTodoDescription) && NewTodoDescription.Length > 1000)
        {
            NewTodoDescriptionError = "Description must be 1000 characters or less";
            return;
        }

        // If we get here, description is valid
        NewTodoDescriptionError = string.Empty;
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public bool IsTodoSelected => SelectedTodo != null;
    public bool IsNotEditing => !IsEditing;

    public ICommand AddTodoCommand { get; }
    public ICommand DeleteTodoCommand { get; }
    public ICommand ToggleCompleteCommand { get; }
    public ICommand EditTodoCommand { get; }
    public ICommand SelectTodoCommand { get; }
    public ICommand SaveTodoCommand { get; }
    public ICommand CancelEditCommand { get; }

    private void LoadTodos()
    {
        var selectedId = SelectedTodo?.Id;
        Todos.Clear();
        var todos = _todoService.GetAllTodos();
        foreach (var todo in todos.OrderByDescending(t => t.CreatedAt))
        {
            var todoVm = new TodoViewModel(todo);
            Todos.Add(todoVm);
            
            // Restore selection if this was the selected todo
            if (selectedId.HasValue && todo.Id == selectedId.Value)
            {
                SelectedTodo = todoVm;
            }
        }
    }

    private void AddTodo()
    {
        // Validate input
        ValidateNewTodoTitle();
        ValidateNewTodoDescription();
        
        if (!string.IsNullOrEmpty(NewTodoTitleError))
        {
            _notificationService.ShowError(NewTodoTitleError);
            return;
        }

        if (!string.IsNullOrEmpty(NewTodoDescriptionError))
        {
            _notificationService.ShowError(NewTodoDescriptionError);
            return;
        }

        try
        {
            var todo = new Todo
            {
                Title = NewTodoTitle.Trim(),
                Description = NewTodoDescription?.Trim() ?? string.Empty,
                CreatedAt = DateTime.Now
            };

            _todoService.AddTodo(todo);
            _notificationService.ShowSuccess("Todo added successfully!");
            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoTitleError = string.Empty; // Clear validation error
            NewTodoDescriptionError = string.Empty; // Clear validation error
            LoadTodos();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Failed to add todo: {ex.Message}");
        }
    }

    private async Task DeleteTodoAsync(TodoViewModel? todo)
    {
        if (todo == null) return;

        // Show confirmation dialog
        var confirmed = await _confirmationService.ShowConfirmation(
            "Delete Todo",
            $"Are you sure you want to delete '{todo.Title}'? This action cannot be undone."
        );

        if (!confirmed) return;

        try
        {
            _todoService.DeleteTodo(todo.Id);
            if (SelectedTodo?.Id == todo.Id)
            {
                SelectedTodo = null;
            }
            _notificationService.ShowSuccess("Todo deleted successfully!");
            LoadTodos();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Failed to delete todo: {ex.Message}");
        }
    }

    private bool CanDeleteTodo(TodoViewModel? todo) => todo != null;

    private void ToggleComplete(TodoViewModel? todo)
    {
        if (todo == null) return;

        try
        {
            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedAt = todo.IsCompleted ? DateTime.Now : null;
            _todoService.UpdateTodo(todo.ToTodo());
            
            var status = todo.IsCompleted ? "completed" : "marked as pending";
            _notificationService.ShowSuccess($"Todo {status}!");
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Failed to update todo: {ex.Message}");
            // Revert the change
            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedAt = todo.IsCompleted ? DateTime.Now : null;
        }
    }

    private void SelectTodo(TodoViewModel? todo)
    {
        if (todo == null) return;
        
        // If already editing, cancel edit mode first
        if (IsEditing)
        {
            IsEditing = false;
        }
        
        SelectedTodo = todo;
    }

    private void EditTodo(TodoViewModel? todo)
    {
        if (todo == null) return;

        SelectedTodo = todo;
        IsEditing = true;
    }

    private void SaveTodo()
    {
        if (SelectedTodo == null) return;

        // Trigger validation
        SelectedTodo.ValidateTitleProperty();
        SelectedTodo.ValidateDescriptionProperty();

        // Check if there are any errors
        if (!string.IsNullOrEmpty(SelectedTodo.TitleError))
        {
            _notificationService.ShowError(SelectedTodo.TitleError);
            return;
        }

        if (!string.IsNullOrEmpty(SelectedTodo.DescriptionError))
        {
            _notificationService.ShowError(SelectedTodo.DescriptionError);
            return;
        }

        try
        {
            var selectedId = SelectedTodo.Id;
            var todoToSave = SelectedTodo.ToTodo();
            
            // Trim whitespace
            todoToSave.Title = todoToSave.Title.Trim();
            todoToSave.Description = todoToSave.Description?.Trim() ?? string.Empty;
            
            // Update completed date if status changed
            if (todoToSave.IsCompleted && todoToSave.CompletedAt == null)
            {
                todoToSave.CompletedAt = DateTime.Now;
            }
            else if (!todoToSave.IsCompleted)
            {
                todoToSave.CompletedAt = null;
            }
            
            _todoService.UpdateTodo(todoToSave);
            _notificationService.ShowSuccess("Todo updated successfully!");
            IsEditing = false;
            
            // Reload and restore selection
            LoadTodos();
            
            // Find and select the updated todo
            var updatedTodo = Todos.FirstOrDefault(t => t.Id == selectedId);
            if (updatedTodo != null)
            {
                SelectedTodo = updatedTodo;
            }
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Failed to save todo: {ex.Message}");
        }
    }

    private void CancelEdit()
    {
        if (SelectedTodo == null) return;
        
        var selectedId = SelectedTodo.Id;
        IsEditing = false;
        LoadTodos(); // Reload to reset any changes
        
        // Restore selection after cancel
        var todo = Todos.FirstOrDefault(t => t.Id == selectedId);
        if (todo != null)
        {
            SelectedTodo = todo;
        }
    }
}

