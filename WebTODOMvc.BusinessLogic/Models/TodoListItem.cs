using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebTODOMvc.BusinessLogic.Models
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

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
