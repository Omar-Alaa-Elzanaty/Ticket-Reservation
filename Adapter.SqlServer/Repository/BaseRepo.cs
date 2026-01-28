using Core.Ports.Repository;

namespace Adapter.SqlServer.Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly TicketDbContext _context;

        public BaseRepo(TicketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            await _context.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
        }

        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}
