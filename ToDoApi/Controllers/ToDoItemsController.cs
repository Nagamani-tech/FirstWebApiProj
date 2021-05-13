using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoItemsController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItemsDTOs.ToListAsync();
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetToDoItem(long id)
        {
            var toDoItem = await _context.TodoItemsDTOs.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return toDoItem;
        }

        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(long id, TodoItemDTO toDoItemDto)
        {
            if (id != toDoItemDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoItemDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostToDoItem(TodoItemDTO todoItemDto)
        {
            _context.TodoItemsDTOs.Add(todoItemDto);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
            return CreatedAtAction(nameof(GetToDoItem), new { id = todoItemDto.Id }, todoItemDto);
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var todoItemDto = await _context.TodoItemsDTOs.FindAsync(id);
            if (todoItemDto == null)
            {
                return NotFound();
            }

            _context.TodoItemsDTOs.Remove(todoItemDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(long id)
        {
            return _context.TodoItemsDTOs.Any(e => e.Id == id);
        }
    }
}
