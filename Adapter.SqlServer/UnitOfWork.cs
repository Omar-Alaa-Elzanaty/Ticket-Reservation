using Core.Ports;
using Core.Ports.Repository;

namespace Adapter.SqlServer
{
    public class UnitOfWork(
        TicketDbContext context,
        ITicketRepo ticket,
        IMatchRepo matchReop) : IUnitOfWork
    {
        private readonly TicketDbContext _context = context;

        public ITicketRepo TicketRepo { get; private set; } = ticket;

        public IMatchRepo MatchRepo { get; private set; } = matchReop;

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}