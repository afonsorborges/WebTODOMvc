using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTODOMvc.BusinessLogic.Services;

namespace WebTODOMvc.UI.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ToDoListService _todoListService;

        private ILogger<TodoController> _logger { get; set; }
        public TodoController(ToDoListService todoListService, ILogger<TodoController> logger)
        {
            _todoListService = todoListService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return View(_todoListService.GetList(userId));
        }

        [HttpGet]
        public IActionResult Create()
        {        
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                try
                {
                    _todoListService.CreateTodoListItem(title, userId);

                    _logger.LogInformation("New note successfully created");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating note: {ex.Message}");
                }
            }

            return RedirectToAction("Index");
        }
    

        [HttpGet]
        public IActionResult Details(int id)
        {
            var findItem = _todoListService.GetTodoListItemById(id);

            if (findItem != null)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (findItem.UserId != userId)
                {
                    return Unauthorized();
                }

                return View(findItem);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var findItem = _todoListService.GetTodoListItemById(id);

            if (findItem != null)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (findItem.UserId != userId)
                {
                    return Unauthorized();
                }
           
                return View(findItem);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(int id, string Title)
        {
            try
            {
                _todoListService.EditTodoListItemById(id, Title);
                _logger.LogInformation("Note successfully edited");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error editing note: {ex.Message}");
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                _todoListService.DeleteTodoListItemById(id);
                _logger.LogInformation("Note successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting note: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckDoneStatus(int id)
        {
            try
            {
                _todoListService.ToggleTodoListItemStatus(id);
                _logger.LogInformation("Done status altered");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error altering done status: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
    }
}
