using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Desktop.Models;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly TodoService _todoService;
    private TodoViewModel? _selectedTodo;

    public MainWindowViewModel()
    {
        _todoService = new TodoService();
        LoadTodos();
        
        AddTodoCommand = new RelayCommand(AddTodo);
        DeleteTodoCommand = new RelayCommand<TodoViewModel>(DeleteTodo, CanDeleteTodo);
        ToggleCompleteCommand = new RelayCommand<TodoViewModel>(ToggleComplete);
        EditTodoCommand = new RelayCommand<TodoViewModel>(EditTodo);
        SelectTodoCommand = new RelayCommand<TodoViewModel>(SelectTodo);
        SaveTodoCommand = new RelayCommand(SaveTodo);
        CancelEditCommand = new RelayCommand(CancelEdit);
    }

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
    public string NewTodoTitle
    {
        get => _newTodoTitle;
        set => SetProperty(ref _newTodoTitle, value);
    }

    private string _newTodoDescription = string.Empty;
    public string NewTodoDescription
    {
        get => _newTodoDescription;
        set => SetProperty(ref _newTodoDescription, value);
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
        if (string.IsNullOrWhiteSpace(NewTodoTitle))
            return;

        var todo = new Todo
        {
            Title = NewTodoTitle.Trim(),
            Description = NewTodoDescription.Trim(),
            CreatedAt = DateTime.Now
        };

        _todoService.AddTodo(todo);
        NewTodoTitle = string.Empty;
        NewTodoDescription = string.Empty;
        LoadTodos();
    }

    private void DeleteTodo(TodoViewModel? todo)
    {
        if (todo == null) return;

        _todoService.DeleteTodo(todo.Id);
        if (SelectedTodo?.Id == todo.Id)
        {
            SelectedTodo = null;
        }
        LoadTodos();
    }

    private bool CanDeleteTodo(TodoViewModel? todo) => todo != null;

    private void ToggleComplete(TodoViewModel? todo)
    {
        if (todo == null) return;

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted ? DateTime.Now : null;
        _todoService.UpdateTodo(todo.ToTodo());
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

        // Validate title is not empty
        if (string.IsNullOrWhiteSpace(SelectedTodo.Title?.Trim()))
        {
            // Title is required - could show error message here
            return;
        }

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

