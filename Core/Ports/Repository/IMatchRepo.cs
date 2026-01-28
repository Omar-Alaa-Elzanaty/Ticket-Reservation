using Core.Features.Matches.Commands.Create;
using Core.Models;

namespace Core.Ports.Repository
{
    public interface IMatchRepo : IBaseRepo<Match>
    {
        Task<bool> IsValidToCreate(CreateMatchCommand command);
        Task<bool> IsFound(Guid id);

    }
}
