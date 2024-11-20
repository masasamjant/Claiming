using Masasamjant.Claiming;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Core
{
    public class Product : IClaimable, IEquatable<Product>
    {
        public Product(Guid identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public Guid Identifier { get; private set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public bool Equals(Product? other)
        {
            return other != null && other.Identifier == Identifier;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Product);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public ClaimKey GetClaimKey()
        {
            return new ClaimKey(this, Identifier.ToString());
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
