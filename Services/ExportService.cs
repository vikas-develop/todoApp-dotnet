using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.Services;

public class ExportService
{
    public async Task ExportToJsonAsync(List<Todo> todos, IStorageFile file)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        var json = JsonSerializer.Serialize(todos, options);
        
        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
    }

    public async Task ExportToCsvAsync(List<Todo> todos, IStorageFile file)
    {
        var lines = new List<string>
        {
            "Id,Title,Description,IsCompleted,CreatedAt,CompletedAt,CategoryId,Priority"
        };

        foreach (var todo in todos)
        {
            var line = $"{todo.Id},\"{EscapeCsvField(todo.Title)}\",\"{EscapeCsvField(todo.Description)}\"," +
                      $"{todo.IsCompleted},{todo.CreatedAt:yyyy-MM-dd HH:mm:ss}," +
                      $"{(todo.CompletedAt.HasValue ? todo.CompletedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")}," +
                      $"{(todo.CategoryId.HasValue ? todo.CategoryId.Value : "")}," +
                      $"{(int)todo.Priority}";
            lines.Add(line);
        }

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        foreach (var line in lines)
        {
            await writer.WriteLineAsync(line);
        }
    }

    private string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return string.Empty;
        
        // Escape quotes by doubling them
        return field.Replace("\"", "\"\"");
    }

    public async Task ExportBackupAsync(List<Todo> todos, List<Category> categories, IStorageFile file)
    {
        var backup = new BackupData
        {
            Version = "1.0",
            ExportDate = DateTime.Now,
            Todos = todos,
            Categories = categories
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        var json = JsonSerializer.Serialize(backup, options);
        
        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
    }
}

public class BackupData
{
    public string Version { get; set; } = string.Empty;
    public DateTime ExportDate { get; set; }
    public List<Todo> Todos { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
}

