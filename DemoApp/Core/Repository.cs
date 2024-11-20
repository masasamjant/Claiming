using System.Collections;
using System.Collections.Concurrent;

namespace DemoApp.Core
{
    public abstract class Repository<T> : IRepository<T>, ICollection<T> where T : class
    {
        protected Repository() 
        { }

        public int Count => Items.Count;

        protected ConcurrentDictionary<T, T> Items { get; } = new ConcurrentDictionary<T, T>();

        public void Add(T item)
        {
            if (!Items.TryAdd(item, item))
                throw new ArgumentException($"The item {item} already added to repository.", nameof(item));
        }

        public void Clear() => Items.Clear();

        public bool Contains(T item) => Items.ContainsKey(item);

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in Items.Values)
                yield return item;
        }

        public abstract void Initialize(string rootFolder);

        public bool Remove(T item) => Items.Remove(item, out var _);

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => Items.Values.CopyTo(array, arrayIndex);
    
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
