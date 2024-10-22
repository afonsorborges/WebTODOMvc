using Microsoft.AspNetCore.Mvc;
using WebTODOMvc.Data;
using WebTODOMvc.Models;

namespace WebTODOMvc.Controllers
{
    public class TodoController : Controller
    {
        private TodoDbContext _context { get; set; }

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }

        [NonAction]
        public IActionResult Hello()
        {
            return Content("Hello");
        }
        public IActionResult Index()
        {
            return View(_context.listItems.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            TodoListItem newItem = new TodoListItem();
            newItem.DateAdded = DateTime.Now;
            newItem.Title = collection["Title"];

            _context.listItems.Add(newItem);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var findItem = _context.listItems
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (findItem != null)
            {
                return View(findItem);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var findItem = _context.listItems
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (findItem != null)
            {
                return View(findItem);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(int id, IFormCollection collection)
        {

            var findItem = _context.listItems
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (findItem != null)
            {
                findItem.DateAdded = DateTime.Now;
                findItem.Title = collection["Title"];


                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {

            var findItem = _context.listItems
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (findItem != null)
            {
                _context.listItems.Remove(findItem);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckTitle(int id)
        {
            var findItem = _context.listItems
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (findItem != null)
            {
                findItem.IsDone = !findItem.IsDone;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
