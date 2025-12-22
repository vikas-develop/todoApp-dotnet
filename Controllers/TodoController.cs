using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers;

public class TodoController : Controller
{
    private readonly TodoDbContext _context;

    public TodoController(TodoDbContext context)
    {
        _context = context;
    }

    // GET: Todo
    public async Task<IActionResult> Index()
    {
        var todos = await _context.Todos
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(todos);
    }

    // GET: Todo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Todo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] Todo todo)
    {
        if (ModelState.IsValid)
        {
            todo.CreatedAt = DateTime.Now;
            _context.Add(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todo);
    }

    // GET: Todo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }
        return View(todo);
    }

    // POST: Todo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted")] Todo todo)
    {
        if (id != todo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingTodo = await _context.Todos.FindAsync(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }

                existingTodo.Title = todo.Title;
                existingTodo.Description = todo.Description;
                existingTodo.IsCompleted = todo.IsCompleted;
                
                if (todo.IsCompleted && !existingTodo.IsCompleted)
                {
                    existingTodo.CompletedAt = DateTime.Now;
                }
                else if (!todo.IsCompleted)
                {
                    existingTodo.CompletedAt = null;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(todo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(todo);
    }

    // POST: Todo/ToggleComplete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted ? DateTime.Now : null;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Todo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todo = await _context.Todos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        return View(todo);
    }

    // POST: Todo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TodoExists(int id)
    {
        return _context.Todos.Any(e => e.Id == id);
    }
}

