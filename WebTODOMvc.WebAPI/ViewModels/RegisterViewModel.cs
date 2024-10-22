using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static NuGet.Packaging.PackagingConstants;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;

namespace WebTODOMvc.WebAPI.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
