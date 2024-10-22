using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTODOMvc.BusinessLogic.Data;
using WebTODOMvc.BusinessLogic.Models;

namespace WebTODOMvc.BusinessLogic.Services
{
    public class ToDoListService
    {
        private readonly TodoDbContext _context;

        public ToDoListService(TodoDbContext context)
        {
            _context = context;
        }

        public List<TodoListItem> GetList(string userId)
        {
            return _context.listItems
                .Where(item => item.UserId == userId)
                .ToList();
        }
        public void CreateTodoListItem(string title, string userId)
        {
            TodoListItem newItem = new TodoListItem
            {
                DateAdded = DateTime.Now,
                Title = title,
                UserId = userId
            };

            _context.listItems.Add(newItem);
            _context.SaveChanges();
        }

        public TodoListItem GetTodoListItemById(int id)
        {
            return _context.listItems
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public bool DeleteTodoListItemById(int id)
        {
            var findItem = _context.listItems
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (findItem != null)
            {
                _context.listItems.Remove(findItem);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public void EditTodoListItemById(int id, string newTitle)
        {
            var findItem = _context.listItems
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (findItem != null)
            {
                findItem.Title = newTitle;
                _context.SaveChanges();
            }
        }
        public void ToggleTodoListItemStatus(int id)
        {
            var findItem = _context.listItems
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (findItem != null)
            {
                findItem.IsDone = !findItem.IsDone;
                _context.SaveChanges();
            }
        }

        public string GetUserIdByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == email);
            return user?.Id;
        }
    }
}
