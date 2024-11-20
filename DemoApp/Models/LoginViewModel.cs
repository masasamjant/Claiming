using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }  = string.Empty;

        public string? RedirectUrl { get; set; }
    }
}
