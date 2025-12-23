using System;
using System.Collections.Generic;
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
    private readonly CategoryService _categoryService;
    private readonly NotificationService _notificationService;
    private readonly ConfirmationService _confirmationService;
    private TodoViewModel? _selectedTodo;

    public MainWindowViewModel()
    {
        _todoService = new TodoService();
        _categoryService = new CategoryService();
        _notificationService = new NotificationService();
        _confirmationService = new ConfirmationService();
        LoadTodos();
        LoadCategories();
        
        AddTodoCommand = new RelayCommand(AddTodo);
        DeleteTodoCommand = new AsyncRelayCommand<TodoViewModel>(DeleteTodoAsync, CanDeleteTodo);
        ToggleCompleteCommand = new RelayCommand<TodoViewModel>(ToggleComplete);
        EditTodoCommand = new RelayCommand<TodoViewModel>(EditTodo);
        SelectTodoCommand = new RelayCommand<TodoViewModel>(SelectTodo);
        SaveTodoCommand = new RelayCommand(SaveTodo);
        CancelEditCommand = new RelayCommand(CancelEdit);
        
        // Navigation commands
        NavigateToTodosCommand = new RelayCommand(() => NavigateToView("Todos"));
        NavigateToCategoriesCommand = new RelayCommand(() => NavigateToView("Categories"));
        NavigateToSettingsCommand = new RelayCommand(() => NavigateToView("Settings"));
        NavigateToAboutCommand = new RelayCommand(() => NavigateToView("About"));
        NewTodoCommand = new RelayCommand(() => NavigateToView("Todos"));
        ExitCommand = new RelayCommand(ExitApplication);
        
        // Start with Todos view
        NavigateToView("Todos");
    }

    public NotificationService NotificationService => _notificationService;
    public ConfirmationService ConfirmationService => _confirmationService;

    private readonly ObservableCollection<TodoViewModel> _allTodos = new();
    public ObservableCollection<TodoViewModel> Todos { get; } = new();

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            ApplyFiltersAndSort();
        }
    }

    private FilterStatus _filterStatus = FilterStatus.All;
    public FilterStatus FilterStatus
    {
        get => _filterStatus;
        set
        {
            SetProperty(ref _filterStatus, value);
            ApplyFiltersAndSort();
        }
    }

    private SortOption _sortOption = SortOption.DateDescending;
    public SortOption SortOption
    {
        get => _sortOption;
        set
        {
            SetProperty(ref _sortOption, value);
            ApplyFiltersAndSort();
        }
    }

    public List<string> FilterStatusOptions => new() { "All", "Pending", "Completed" };
    public List<string> SortOptions => new() 
    { 
        "Newest First", 
        "Oldest First", 
        "Title (A-Z)", 
        "Title (Z-A)", 
        "Status (Pending First)", 
        "Status (Completed First)" 
    };

    private string _selectedFilterStatusText = "All";
    public string SelectedFilterStatusText
    {
        get => _selectedFilterStatusText;
        set
        {
            SetProperty(ref _selectedFilterStatusText, value);
            FilterStatus = value switch
            {
                "Pending" => FilterStatus.Pending,
                "Completed" => FilterStatus.Completed,
                _ => FilterStatus.All
            };
        }
    }

    private string _selectedSortOptionText = "Newest First";
    public string SelectedSortOptionText
    {
        get => _selectedSortOptionText;
        set
        {
            SetProperty(ref _selectedSortOptionText, value);
            SortOption = value switch
            {
                "Oldest First" => SortOption.DateAscending,
                "Title (A-Z)" => SortOption.TitleAscending,
                "Title (Z-A)" => SortOption.TitleDescending,
                "Status (Pending First)" => SortOption.StatusAscending,
                "Status (Completed First)" => SortOption.StatusDescending,
                _ => SortOption.DateDescending
            };
        }
    }

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
    private int? _newTodoCategoryId;
    
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

    public int? NewTodoCategoryId
    {
        get => _newTodoCategoryId;
        set => SetProperty(ref _newTodoCategoryId, value);
    }

    private Category? _newTodoCategory;
    public Category? NewTodoCategory
    {
        get => _newTodoCategory;
        set
        {
            SetProperty(ref _newTodoCategory, value);
            NewTodoCategoryId = value?.Id;
        }
    }

    public ObservableCollection<Category> Categories { get; } = new();
    
    private Category? _selectedCategoryForFilter;
    public Category? SelectedCategoryForFilter
    {
        get => _selectedCategoryForFilter;
        set
        {
            SetProperty(ref _selectedCategoryForFilter, value);
            ApplyFiltersAndSort();
        }
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
    public bool HasTodos => Todos.Count > 0;

    // Navigation properties
    private string _currentView = "Todos";
    public bool IsTodosView => _currentView == "Todos";
    public bool IsCategoriesView => _currentView == "Categories";
    public bool IsSettingsView => _currentView == "Settings";
    public bool IsAboutView => _currentView == "About";

    private void NavigateToView(string viewName)
    {
        _currentView = viewName;
        OnPropertyChanged(nameof(IsTodosView));
        OnPropertyChanged(nameof(IsCategoriesView));
        OnPropertyChanged(nameof(IsSettingsView));
        OnPropertyChanged(nameof(IsAboutView));
    }

    private void ExitApplication()
    {
        Environment.Exit(0);
    }

    public ICommand AddTodoCommand { get; }
    public ICommand DeleteTodoCommand { get; }
    public ICommand ToggleCompleteCommand { get; }
    public ICommand EditTodoCommand { get; }
    public ICommand SelectTodoCommand { get; }
    public ICommand SaveTodoCommand { get; }
    public ICommand CancelEditCommand { get; }
    
    // Navigation commands
    public ICommand NavigateToTodosCommand { get; }
    public ICommand NavigateToCategoriesCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }
    public ICommand NavigateToAboutCommand { get; }
    public ICommand NewTodoCommand { get; }
    public ICommand ExitCommand { get; }

    private void LoadTodos()
    {
        var selectedId = SelectedTodo?.Id;
        _allTodos.Clear();
        var todos = _todoService.GetAllTodos();
        foreach (var todo in todos)
        {
            var todoVm = new TodoViewModel(todo);
            _allTodos.Add(todoVm);
        }
        
        ApplyFiltersAndSort();
        
        // Restore selection if this was the selected todo
        if (selectedId.HasValue)
        {
            var restoredTodo = Todos.FirstOrDefault(t => t.Id == selectedId.Value);
            if (restoredTodo != null)
            {
                SelectedTodo = restoredTodo;
            }
        }
    }

    private void LoadCategories()
    {
        Categories.Clear();
        var categories = _categoryService.GetAllCategories();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    private void ApplyFiltersAndSort()
    {
        var selectedId = SelectedTodo?.Id;
        var query = _allTodos.AsEnumerable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLowerInvariant();
            query = query.Where(t => 
                t.Title.ToLowerInvariant().Contains(searchLower) ||
                (!string.IsNullOrEmpty(t.Description) && t.Description.ToLowerInvariant().Contains(searchLower))
            );
        }

        // Apply status filter
        if (FilterStatus == FilterStatus.Pending)
        {
            query = query.Where(t => !t.IsCompleted);
        }
        else if (FilterStatus == FilterStatus.Completed)
        {
            query = query.Where(t => t.IsCompleted);
        }

        // Apply sorting
        query = SortOption switch
        {
            SortOption.DateDescending => query.OrderByDescending(t => t.CreatedAt),
            SortOption.DateAscending => query.OrderBy(t => t.CreatedAt),
            SortOption.TitleAscending => query.OrderBy(t => t.Title),
            SortOption.TitleDescending => query.OrderByDescending(t => t.Title),
            SortOption.StatusAscending => query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt),
            SortOption.StatusDescending => query.OrderByDescending(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        // Update the displayed collection
        Todos.Clear();
        foreach (var todo in query)
        {
            Todos.Add(todo);
        }
        
        OnPropertyChanged(nameof(HasTodos));

        // Restore selection if it still exists
        if (selectedId.HasValue)
        {
            var restoredTodo = Todos.FirstOrDefault(t => t.Id == selectedId.Value);
            if (restoredTodo != null)
            {
                SelectedTodo = restoredTodo;
            }
            else
            {
                SelectedTodo = null; // Selected todo was filtered out
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
                CreatedAt = DateTime.Now,
                CategoryId = NewTodoCategoryId
            };

            _todoService.AddTodo(todo);
            _notificationService.ShowSuccess("Todo added successfully!");
        NewTodoTitle = string.Empty;
        NewTodoDescription = string.Empty;
        NewTodoTitleError = string.Empty;
        NewTodoCategoryId = null;
        NewTodoCategory = null; // Clear validation error
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
            todoToSave.CategoryId = SelectedTodo.Category?.Id ?? SelectedTodo.CategoryId;
            
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

