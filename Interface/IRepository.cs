namespace Sales_Management.Interface
{
    public interface IRepository<K, T>
    {
        public Task<List<T>?> GetAll();
        public Task<T?> Get(K key);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T?> Delete(K key);
    }
}
