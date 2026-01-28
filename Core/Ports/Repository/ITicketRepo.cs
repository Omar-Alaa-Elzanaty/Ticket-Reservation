using Core.Dtos;
using Core.Models;

namespace Core.Ports.Repository
{
    public interface ITicketRepo : IBaseRepo<Ticket>
    {
        Task<bool> IsAlreadyCreated(string SetNumber,Guid matchId);
        Task<bool> ReserveTicket(int ticketId, string userId);
    }
}
