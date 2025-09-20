using Masasamjant.Claiming;

namespace DemoApp.Core
{
    public class ProductService : ClaimingService, IService<Guid, Product>
    {
        private readonly IRepository<Product> repository;

        public ProductService(IRepository<Product> repository, IClaimManagerFactory claimManagerFactory, IConfiguration configuration)
            : base(claimManagerFactory, configuration)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAsync(Product instance)
        {
            var currentProduct = await FindAsync(instance.Identifier);

            if (currentProduct != null)
                return false;

            repository.Add(instance);
            return true;
        }

        public async Task<IClaimDescriptor> EditAsync(Product instance, IClaim claim)
        {
            var currentClaim = await ClaimManager.GetClaimAsync(instance.GetClaimKey());

            if (currentClaim == null)
                return new ClaimDescriptor(ClaimResult.NotFound, null);

            if (!currentClaim.Equals(claim))
                return new ClaimDescriptor(ClaimResult.Denied, currentClaim);

            var currentProduct = await FindAsync(instance.Identifier);

            if (currentProduct == null)
                return new ClaimDescriptor(ClaimResult.NotFound, null);

            currentProduct.Name = instance.Name;
            currentProduct.Description = instance.Description;

            return new ClaimDescriptor(ClaimResult.Approved, currentClaim);
        }

        public Task<Product?> FindAsync(Guid identifier)
        {
            return Task.FromResult(repository.FirstOrDefault(x => x.Identifier == identifier));
        }

        public Task<IEnumerable<Product>> ListAsync()
        {
            return Task.FromResult(repository.ToList().AsEnumerable());
        }
    }
}
