using Core.Dtos;
using Core.Models;
using Core.Ports;
using Mapster;
using MediatR;

namespace Core.Features.Tickets.Commands.Create
{
    public class CreateTicketCommand : IRequest<BaseResponse<int>>
    {
        public Guid MatchId { get; set; }
        public string SetNumber { get; set; }
    }

    internal class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<int>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.MatchRepo.IsFound(request.MatchId))
            {
                return BaseResponse<int>.Failure("Match not found", System.Net.HttpStatusCode.NotFound);
            }

            if (await _unitOfWork.TicketRepo.IsAlreadyCreated(request.SetNumber, request.MatchId))
            {
                return BaseResponse<int>.Failure("No tickets available for this match", System.Net.HttpStatusCode.BadRequest);
            }

            var ticket = request.Adapt<Ticket>();

            await _unitOfWork.TicketRepo.AddAsync(ticket);

            await _unitOfWork.CommitAsync(cancellationToken);

            return BaseResponse<int>.Success(ticket.Id, "Ticket created successfully", System.Net.HttpStatusCode.Created);
        }
    }
}
