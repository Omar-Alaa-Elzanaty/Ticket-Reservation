using Core.Features.Matches.Commands.Create;
using Core.Models;
using Core.Ports.Repository;
using Microsoft.EntityFrameworkCore;

namespace Adapter.SqlServer.Repository
{
    public class MatchRepo : BaseRepo<Match>, IMatchRepo
    {
        private readonly TicketDbContext _context;

        public MatchRepo(TicketDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsFound(Guid id)
        {
            return await _context.Matches.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> IsValidToCreate(CreateMatchCommand command)
        {

            return !await _context.Matches
                .AnyAsync(x => ((x.TeamA == command.TeamA && x.TeamB == command.TeamB)
                || (x.TeamA == command.TeamB && x.TeamB == command.TeamA))
                && (x.MatchDate.Date != command.MatchDate.Date));
        }
    }
}