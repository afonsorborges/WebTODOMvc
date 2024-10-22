using System.ComponentModel.DataAnnotations;

namespace WebTODOMvc.Models
{
    public class TodoListItem
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Title must contain at least two characters!")]
        [MaxLength(200, ErrorMessage = "Title must contain a maximum of 200 characters!")]
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }
}
