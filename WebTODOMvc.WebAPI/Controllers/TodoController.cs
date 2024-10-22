using System.Security.Claims;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTODOMvc.BusinessLogic.Data;
using WebTODOMvc.BusinessLogic.Models;
using WebTODOMvc.BusinessLogic.Services;

namespace WebTODOMvc.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class TodoController : ControllerBase
{     
    private readonly ToDoListService _todoListService;
 
    public TodoController(ToDoListService todoListService)
    {
        _todoListService = todoListService;  
    }

    [HttpGet("Index")] 
    public IActionResult GetTodoItems()
    {
        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (email != null)
        {
            var userId = _todoListService.GetUserIdByEmail(email);
            var todoItems = _todoListService.GetList(userId);
            return Ok(todoItems);
        }
        else
        {
            return NotFound("User not found");
        }
    }

    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string userId = _todoListService.GetUserIdByEmail(email);

        var todoItem = _todoListService.GetTodoListItemById(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        if (todoItem.UserId != userId)
        {
            return Unauthorized();
        }

        return Ok(todoItem);
    }

    [HttpPost("Create")]
    public IActionResult Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Title cannot be empty.");
        }

        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (email != null)
        {
            var userId = _todoListService.GetUserIdByEmail(email);

            try
            {
                _todoListService.CreateTodoListItem(title, userId);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating todo item: {ex.Message}");
            }
        }
        else
        {
            return NotFound("User not found");
        }

       
    }


    [HttpPut("Edit/{id}")]
    public IActionResult UpdateTodoItem(int id, [FromBody] string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            return BadRequest("Title cannot be empty.");
        }

        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string userId = _todoListService.GetUserIdByEmail(email);

        var existingItem = _todoListService.GetTodoListItemById(id);
        if (existingItem == null)
        {
            return NotFound(); 
        }

        if (existingItem.UserId != userId)
        {
            return Unauthorized();
        }

        try
        {
            _todoListService.EditTodoListItemById(id, newTitle);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating todo item: {ex.Message}");
        }
    }

    [HttpPut("ToggleDoneStatus/{id}")]
    public IActionResult ToggleTodoItemStatus(int id)
    {
        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string userId = _todoListService.GetUserIdByEmail(email);

        var existingItem = _todoListService.GetTodoListItemById(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        if (existingItem.UserId != userId)
        {
            return Unauthorized();
        }

        _todoListService.ToggleTodoListItemStatus(id);

        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult DeleteTodoItem(int id)
    {
        string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string userId = _todoListService.GetUserIdByEmail(email);

        var existingItem = _todoListService.GetTodoListItemById(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        if (existingItem.UserId != userId)
        {
            return Unauthorized();
        }

        var deleted = _todoListService.DeleteTodoListItemById(id);

        return deleted ? NoContent() : NotFound();
    }
}
