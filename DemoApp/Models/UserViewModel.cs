using Masasamjant.Claiming;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class UserViewModel : ClaimViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(30)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }
    }
}
