using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists/{listId}/items")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }
        
        // GET: api/todolists/{listId}/items/{itemId}
        [HttpGet("{itemId}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long listId, long itemId)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(x => x.TodoListId == listId && x.Id == itemId);
            if (item == null)
            {
                return NotFound();
            }
            
            return Ok(item);
        }
        
        // POST: api/todolists/{listId}/items
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItem(long listId, CreateTodoItem dto)
        {
            var todoList = await _context.TodoList.FindAsync(listId);
            if (todoList == null)
            {
                return NotFound();
            }

            var item = new TodoItem
            {
                Description = dto.Description,
                IsCompleted = false,
                TodoListId = listId
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            var resultDto = new TodoItemDto
            {
                Id = item.Id,
                Description = item.Description,
                IsCompleted = item.IsCompleted,
                TodoListId = listId
            };
            
            return CreatedAtAction(nameof(GetTodoItem), new { listId = listId, itemId = item.Id }, resultDto);
        }
        
        // PUT: api/todolists/{listId}/items/{itemId}
        [HttpPut("{itemId}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long listId, long itemId, UpdateTodoItem dto)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(x => x.TodoListId == listId && x.Id == itemId);
            if (item == null)
            {
                return NotFound();
            }
            
            item.Description = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(item);
        }
        
        // PATCH: api/todolists/{listId}/items/{itemId}/complete
        [HttpPatch("{itemId}/complete")]
        public async Task<IActionResult> CompleteTodoItem(long listId, long itemId)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(x => x.TodoListId == listId && x.Id == itemId);
            if (item == null)
            {
                return NotFound();
            }
            
            item.IsCompleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        // DELETE: api/todolists/{listId}/items/{itemId}
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteTodoItem(long listId, long itemId)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(x => x.TodoListId == listId && x.Id == itemId);
            if (item == null)
            {
                return NotFound();
            }
            
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}