using Core.Enums;
using Core.Models;
using Core.Ports.Repository;
using Microsoft.EntityFrameworkCore;

namespace Adapter.SqlServer.Repository
{
    public class TicketRepo : BaseRepo<Ticket>, ITicketRepo
    {
        private readonly TicketDbContext _context;

        public TicketRepo(TicketDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> IsAlreadyCreated(string SetNumber, Guid matchId)
        {
            return _context.Tickets.AnyAsync(t => t.SetNumber == SetNumber && t.MatchId == matchId);
        }

        public async Task<bool> ReserveTicket(int ticketId, string userId)
        {
            if (await _context.Tickets.AnyAsync(t => t.Id == ticketId && t.Status == TicketStatus.Available))
            {

                Reservation entity = new()
                {
                    TicketId = ticketId,
                    UserId = userId
                };

                await _context.Reservations.AddAsync(entity);
                await _context.Tickets.Where(x => x.Id == ticketId)
                    .ExecuteUpdateAsync(t => t.SetProperty(t => t.Status, TicketStatus.Sold));
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
