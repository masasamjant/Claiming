using Masasamjant.Claiming;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Core
{
    public class User : IClaimable, IEquatable<User>
    {
        public User(string userName)
        {
            UserName = userName;
        }

        [Required]
        [MaxLength(30)]
        public string UserName { get; private set; }

        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }

        public ClaimKey GetClaimKey()
        {
            return new ClaimKey(this, UserName, "DemoApp");
        }

        public bool Equals(User? other)
        {
            return other != null && UserName == other.UserName;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return UserName.GetHashCode();
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}
