using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApp.Desktop.Data;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.Services;

public class TodoService
{
    private readonly TodoDbContext _context;

    public TodoService()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite("Data Source=todos_desktop.db")
            .Options;
        _context = new TodoDbContext(options);
        _context.Database.EnsureCreated();
    }

    public List<Todo> GetAllTodos()
    {
        return _context.Todos.Include(t => t.Category).ToList();
    }

    public Todo? GetTodoById(int id)
    {
        return _context.Todos.Find(id);
    }

    public void AddTodo(Todo todo)
    {
        _context.Todos.Add(todo);
        _context.SaveChanges();
    }

    public void UpdateTodo(Todo todo)
    {
        var existing = _context.Todos.Find(todo.Id);
        if (existing != null)
        {
            existing.Title = todo.Title;
            existing.Description = todo.Description;
            existing.IsCompleted = todo.IsCompleted;
            existing.CompletedAt = todo.CompletedAt;
            existing.CategoryId = todo.CategoryId;
            _context.SaveChanges();
        }
    }

    public List<Todo> GetAllTodosWithCategories()
    {
        return _context.Todos.Include(t => t.Category).ToList();
    }

    public void DeleteTodo(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            _context.SaveChanges();
        }
    }
}

