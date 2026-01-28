namespace Core.Ports.Repository
{
    public interface IBaseRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
    }
}
