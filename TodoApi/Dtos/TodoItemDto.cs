namespace TodoApi.Dtos;

public class TodoItemDto
{
    public long Id { get; set; }
    public string Description { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public long TodoListId { get; set; }
}