using Core.Ports.Repository;

namespace Core.Ports
{
    public interface IUnitOfWork
    {
        ITicketRepo TicketRepo { get; }
        IMatchRepo MatchRepo { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}
