namespace Application.Infrastructure.Persistence;

using Application.Domain.Todo.Models;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<TodoList> TodoList { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
}
