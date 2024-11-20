using DemoApp.Core;

namespace DemoApp.Models
{
    public class UsersViewModel
    {
        public UsersViewModel(IEnumerable<User> users)
        {
            Users = users;
        }

        public IEnumerable<User> Users { get; }
    }
}
