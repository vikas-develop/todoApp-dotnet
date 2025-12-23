using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.Services;

public class ImportService
{
    private readonly TodoService _todoService;
    private readonly CategoryService _categoryService;

    public ImportService(TodoService todoService, CategoryService categoryService)
    {
        _todoService = todoService;
        _categoryService = categoryService;
    }

    public ImportResult ImportFromJson(string filePath, bool overwriteExisting = false)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var todos = JsonSerializer.Deserialize<List<Todo>>(json);
            
            if (todos == null)
                return new ImportResult { Success = false, Message = "Invalid JSON file format" };

            return ImportTodos(todos, overwriteExisting);
        }
        catch (Exception ex)
        {
            return new ImportResult { Success = false, Message = $"Error importing JSON: {ex.Message}" };
        }
    }

    public async Task<ImportResult> ImportFromJsonAsync(IStorageFile file, bool overwriteExisting = false)
    {
        try
        {
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var todos = JsonSerializer.Deserialize<List<Todo>>(json);
            
            if (todos == null)
                return new ImportResult { Success = false, Message = "Invalid JSON file format" };

            return ImportTodos(todos, overwriteExisting);
        }
        catch (Exception ex)
        {
            return new ImportResult { Success = false, Message = $"Error importing JSON: {ex.Message}" };
        }
    }

    public async Task<ImportResult> ImportFromCsvAsync(IStorageFile file, bool overwriteExisting = false)
    {
        try
        {
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            var lines = new List<string>();
            
            while (await reader.ReadLineAsync() is { } line)
            {
                lines.Add(line);
            }
            
            if (lines.Count < 2)
                return new ImportResult { Success = false, Message = "CSV file is empty or invalid" };

            var todos = new List<Todo>();
            
            // Skip header line
            for (int i = 1; i < lines.Count; i++)
            {
                var todo = ParseCsvLine(lines[i]);
                if (todo != null)
                    todos.Add(todo);
            }

            return ImportTodos(todos, overwriteExisting);
        }
        catch (Exception ex)
        {
            return new ImportResult { Success = false, Message = $"Error importing CSV: {ex.Message}" };
        }
    }

    public async Task<ImportResult> RestoreFromBackupAsync(IStorageFile file, bool overwriteExisting = false)
    {
        try
        {
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var backup = JsonSerializer.Deserialize<BackupData>(json);
            
            if (backup == null)
                return new ImportResult { Success = false, Message = "Invalid backup file format" };

            var result = new ImportResult { Success = true, ImportedTodos = 0, ImportedCategories = 0 };

            // Import categories first
            if (backup.Categories != null && backup.Categories.Any())
            {
                foreach (var category in backup.Categories)
                {
                    try
                    {
                        var existing = _categoryService.GetCategoryById(category.Id);
                        if (existing == null)
                        {
                            _categoryService.AddCategory(category.Name, category.Color);
                            result.ImportedCategories++;
                        }
                        else if (overwriteExisting)
                        {
                            _categoryService.UpdateCategory(category);
                            result.ImportedCategories++;
                        }
                    }
                    catch
                    {
                        // Skip duplicate or invalid categories
                    }
                }
            }

            // Import todos
            if (backup.Todos != null)
            {
                var todoResult = ImportTodos(backup.Todos, overwriteExisting);
                result.ImportedTodos = todoResult.ImportedTodos;
                result.Success = todoResult.Success;
                if (!string.IsNullOrEmpty(todoResult.Message))
                    result.Message = todoResult.Message;
            }

            return result;
        }
        catch (Exception ex)
        {
            return new ImportResult { Success = false, Message = $"Error restoring backup: {ex.Message}" };
        }
    }

    private ImportResult ImportTodos(List<Todo> todos, bool overwriteExisting)
    {
        var result = new ImportResult { Success = true, ImportedTodos = 0 };

        foreach (var todo in todos)
        {
            try
            {
                // Reset ID to let database assign new one
                todo.Id = 0;
                
                if (overwriteExisting)
                {
                    var existing = _todoService.GetTodoById(todo.Id);
                    if (existing != null)
                    {
                        todo.Id = existing.Id;
                        _todoService.UpdateTodo(todo);
                        result.ImportedTodos++;
                        continue;
                    }
                }

                _todoService.AddTodo(todo);
                result.ImportedTodos++;
            }
            catch
            {
                // Skip invalid todos
            }
        }

        return result;
    }

    private Todo? ParseCsvLine(string line)
    {
        try
        {
            var parts = ParseCsvLineParts(line);
            if (parts.Length < 5)
                return null;

            var todo = new Todo
            {
                Title = UnescapeCsvField(parts[1]),
                Description = UnescapeCsvField(parts[2]),
                IsCompleted = bool.Parse(parts[3]),
                CreatedAt = DateTime.Parse(parts[4])
            };

            if (parts.Length > 5 && !string.IsNullOrEmpty(parts[5]))
                todo.CompletedAt = DateTime.Parse(parts[5]);

            if (parts.Length > 6 && !string.IsNullOrEmpty(parts[6]) && int.TryParse(parts[6], out var categoryId))
                todo.CategoryId = categoryId;

            if (parts.Length > 7 && !string.IsNullOrEmpty(parts[7]) && int.TryParse(parts[7], out var priorityInt) && Enum.IsDefined(typeof(PriorityLevel), priorityInt))
                todo.Priority = (PriorityLevel)priorityInt;

            return todo;
        }
        catch
        {
            return null;
        }
    }

    private string[] ParseCsvLineParts(string line)
    {
        var parts = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                parts.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        parts.Add(current.ToString());

        return parts.ToArray();
    }

    private string UnescapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return string.Empty;
        
        // Remove surrounding quotes if present
        if (field.StartsWith("\"") && field.EndsWith("\""))
            field = field.Substring(1, field.Length - 2);
        
        // Unescape doubled quotes
        return field.Replace("\"\"", "\"");
    }
}

public class ImportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ImportedTodos { get; set; }
    public int ImportedCategories { get; set; }
}

