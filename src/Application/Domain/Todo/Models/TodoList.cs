namespace Application.Domain.Todo.Models;

using Application.Domain.Todo.ValueObjects;

public class TodoList
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public Color Color { get; set; } = Color.White;

    public IList<TodoItem> Items { get; init; } = [];
}
