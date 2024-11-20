namespace DemoApp.Core
{
    public interface IRepository<T> : ICollection<T> where T : class
    {
    }
}
