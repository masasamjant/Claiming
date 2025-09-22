using Masasamjant.Claiming;

namespace DemoApp.Core
{
    public interface IService<TIdentifier, T> : IClaimingService where T : class
    {
        Task<bool> AddAsync(T instance);

        Task<ClaimDescriptor> EditAsync(T instance, Claim claim);

        Task<T?> FindAsync(TIdentifier identifier);

        Task<IEnumerable<T>> ListAsync();
    }
}
