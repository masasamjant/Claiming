using Masasamjant.Claiming;

namespace DemoApp.Core
{
    public interface IService<TIdentifier, T> : IClaimingService where T : class
    {
        Task<bool> AddAsync(T instance);

        Task<IClaimDescriptor> EditAsync(T instance, IClaim claim);

        Task<T?> FindAsync(TIdentifier identifier);

        Task<IEnumerable<T>> ListAsync();
    }
}
