using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi.Tests;

public class TodoItemsControllerTests
{
    private DbContextOptions<TodoContext> DatabaseContextOptions()
    {
        return new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    private void PopulateDatabaseContext(TodoContext context)
    {
        context.TodoList.Add(new TodoList { Id = 1, Name = "Test List" });
        context.SaveChanges();
    }
    
    [Fact]
    public async Task GetTodoItem_WhenCalled_ReturnsTodoItemById()
    {
        using (var context = new TodoContext(DatabaseContextOptions()))
        {
            PopulateDatabaseContext(context);
            context.TodoItems.Add(new TodoItem
            {
                Id = 1,
                ListId = 1,
                Description = "Task X",
                IsCompleted = false
            });
            context.SaveChanges();

            var controller = new TodoItemsController(context);

            var result = await controller.GetTodoItem(1, 1);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(1, ((result.Result as OkObjectResult).Value as TodoItem).Id);
        }
    }
    
    [Fact]
    public async Task GetTodoItem_WhenItemDoesNotExist_ReturnsNotFoundResult()
    {
        using (var context = new TodoContext(DatabaseContextOptions()))
        {
            PopulateDatabaseContext(context);

            var controller = new TodoItemsController(context);

            var result = await controller.GetTodoItem(1, 999);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    [Fact]
    public async Task CreateItem_WhenCalled_CreatesTodoItem()
    {
        using (var context = new TodoContext(DatabaseContextOptions()))
        {
            PopulateDatabaseContext(context);

            var controller = new TodoItemsController(context);
            
            var result = await controller.PostTodoItem(1, new Dtos.CreateTodoItem { Description = "New Task" });

            Assert.IsType<CreatedAtActionResult>(result.Result);

            Assert.Equal(1, context.TodoItems.Count());
        }
    }

    [Fact]
    public async Task UpdateItem_WhenItemDoesNotExist_ReturnsNotFoundResult()
    {
        using (var context = new TodoContext(DatabaseContextOptions()))
        {
            PopulateDatabaseContext(context);

            var controller = new TodoItemsController(context);
            var dto = new Dtos.UpdateTodoItem { Description = "New Description" };

            var result = await controller.PutTodoItem(1, 999, dto);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    [Fact]
    public async Task UpdateItem_WhenCalled_UpdatesTodoItem()
    {
        using (var context = new TodoContext(DatabaseContextOptions()))
        {
            PopulateDatabaseContext(context);

            context.TodoItems.Add(new TodoItem
            {
                Id = 2,
                ListId = 1,
                Description = "Description",
                IsCompleted = false
            });
            context.SaveChanges();

            var controller = new TodoItemsController(context);
            var dto = new Dtos.UpdateTodoItem { Description = "New Description" };

            var result = await controller.PutTodoItem(1, 2, dto);
            
            Assert.IsType<OkObjectResult>(result.Result);
            
            var updated = context.TodoItems.First(x => x.Id == 2);
            Assert.Equal("New Description", updated.Description);
        }
    }
    
    [Fact]
    public async Task CompleteItem_WhenItemDoesNotExist_ReturnsNotFoundResult()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(context);

        var result = await controller.CompleteTodoItem(1, 999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CompleteItem_WhenCalled_MarksItemAsCompleted()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        context.TodoItems.Add(new TodoItem
        {
            Id = 5,
            ListId = 1,
            Description = "Pending Task",
            IsCompleted = false
        });
        context.SaveChanges();

        var controller = new TodoItemsController(context);

        var result = await controller.CompleteTodoItem(1, 5);
        
        Assert.IsType<NoContentResult>(result);
        
        var updated = context.TodoItems.Single(x => x.Id == 5);
        Assert.True(updated.IsCompleted);
    }
    
    [Fact]
    public async Task DeleteItem_WhenItemDoesNotExist_ReturnsNotFoundResult()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(context);
        var result = await controller.DeleteTodoItem(1, 999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteItem_WhenCalled_RemovesTodoItem()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        context.TodoItems.Add(new TodoItem
        {
            Id = 7,
            ListId = 1,
            Description = "Task to Delete",
            IsCompleted = false
        });
        context.SaveChanges();

        var controller = new TodoItemsController(context);
        var result = await controller.DeleteTodoItem(1, 7);

        Assert.IsType<NoContentResult>(result);

        Assert.False(context.TodoItems.Any(x => x.Id == 7));
    }
}