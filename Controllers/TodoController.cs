
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp.Data;
using TodoApp.DTOs;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var todos = await _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t => new TodoDto
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    IsCompleted = t.IsCompleted
                })
                .ToListAsync();

            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] TodoDto todoDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var todo = new Todo
            {
                TaskName = todoDto.TaskName,
                IsCompleted = false,
                UserId = userId
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            todoDto.Id = todo.Id;
            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoDto todoDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
                return NotFound();

            todo.TaskName = todoDto.TaskName;
            todo.IsCompleted = todoDto.IsCompleted;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
                return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
