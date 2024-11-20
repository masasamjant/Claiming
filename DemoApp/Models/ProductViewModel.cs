using Masasamjant.Claiming;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class ProductViewModel : ClaimViewModel
    {
        public Guid Identifier { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
