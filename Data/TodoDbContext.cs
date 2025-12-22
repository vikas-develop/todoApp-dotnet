using Microsoft.EntityFrameworkCore;
using TodoApp.Desktop.Models;

namespace TodoApp.Desktop.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }
}
