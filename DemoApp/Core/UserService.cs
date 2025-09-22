using Masasamjant.Claiming;

namespace DemoApp.Core
{
    public class UserService : ClaimingService, IService<string, User>
    {
        private readonly IRepository<User> repository;

        public UserService(IRepository<User> repository, IClaimManagerFactory claimManagerFactory, IConfiguration configuration)
            : base(claimManagerFactory, configuration)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAsync(User instance)
        {
            var currentUser = await FindAsync(instance.UserName);

            if (currentUser != null)
                return false;

            repository.Add(instance);
            return true;
        }

        public async Task<ClaimDescriptor> EditAsync(User instance, Claim claim)
        {
            var currentClaim = await ClaimManager.GetClaimAsync(instance.GetClaimKey());

            if (currentClaim == null)
                return new ClaimDescriptor(ClaimResult.NotFound, null);

            if (!currentClaim.Equals(claim))
                return new ClaimDescriptor(ClaimResult.Denied, currentClaim);

            var currentUser = await FindAsync(instance.UserName);

            if (currentUser == null)
                return new ClaimDescriptor(ClaimResult.NotFound, null);

            currentUser.FirstName = instance.FirstName;
            currentUser.LastName = instance.LastName;

            return new ClaimDescriptor(ClaimResult.Approved, currentClaim);
        }

        public Task<User?> FindAsync(string identifier)
        {
            return Task.FromResult(repository.FirstOrDefault(x => x.UserName == identifier));   
        }

        public Task<IEnumerable<User>> ListAsync()
        {
            return Task.FromResult(repository.ToList().AsEnumerable());
        }
    }
}
