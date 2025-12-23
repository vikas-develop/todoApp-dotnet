using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Desktop.Data;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.Services;

public class CategoryService
{
    private readonly TodoDbContext _context;

    public CategoryService()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite("Data Source=todos_desktop.db")
            .Options;
        _context = new TodoDbContext(options);
        
        // Ensure database and all tables are created
        _context.Database.EnsureCreated();
        
        // Check if Categories table exists, if not, create it
        try
        {
            _context.Database.ExecuteSqlRaw("SELECT COUNT(*) FROM Categories LIMIT 1");
        }
        catch
        {
            // Table doesn't exist, create it
            _context.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Color TEXT NOT NULL
                )");
        }
        
        // Also ensure Todos table has CategoryId column
        try
        {
            _context.Database.ExecuteSqlRaw("SELECT CategoryId FROM Todos LIMIT 1");
        }
        catch
        {
            // Column doesn't exist, add it
            _context.Database.ExecuteSqlRaw("ALTER TABLE Todos ADD COLUMN CategoryId INTEGER");
        }
        
        InitializeDefaultCategories();
    }

    private void InitializeDefaultCategories()
    {
        if (!_context.Categories.Any())
        {
            var defaultCategories = new List<Category>
            {
                new Category { Name = "Work", Color = "#3B82F6" },      // Blue
                new Category { Name = "Personal", Color = "#10B981" },   // Green
                new Category { Name = "Shopping", Color = "#F59E0B" },   // Amber
                new Category { Name = "Health", Color = "#EF4444" },     // Red
                new Category { Name = "Learning", Color = "#8B5CF6" },   // Purple
                new Category { Name = "Other", Color = "#6366F1" }       // Indigo
            };

            _context.Categories.AddRange(defaultCategories);
            _context.SaveChanges();
        }
    }

    public List<Category> GetAllCategories()
    {
        return _context.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category? GetCategoryById(int id)
    {
        return _context.Categories.Find(id);
    }

    public Category AddCategory(string name, string color)
    {
        var category = new Category { Name = name, Color = color };
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category;
    }

    public void UpdateCategory(Category category)
    {
        var existing = _context.Categories.Find(category.Id);
        if (existing != null)
        {
            existing.Name = category.Name;
            existing.Color = category.Color;
            _context.SaveChanges();
        }
    }

    public void DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            // Set todos with this category to null
            var todos = _context.Todos.Where(t => t.CategoryId == id).ToList();
            foreach (var todo in todos)
            {
                todo.CategoryId = null;
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}

