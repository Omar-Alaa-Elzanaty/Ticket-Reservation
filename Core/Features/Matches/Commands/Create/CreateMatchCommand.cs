using Core.Dtos;
using Core.Models;
using Core.Ports;
using Mapster;
using MediatR;
using System.Net;

namespace Core.Features.Matches.Commands.Create
{
    public class CreateMatchCommand : IRequest<BaseResponse<Guid>>
    {
        public DateTimeOffset MatchDate { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
    }

    internal class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, BaseResponse<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMatchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<Guid>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.MatchRepo.IsValidToCreate(command))
            {
                return BaseResponse<Guid>.Failure("Conflict between matches time.",HttpStatusCode.BadRequest);
            }

            var match = command.Adapt<Match>();

            await _unitOfWork.MatchRepo.AddAsync(match);
            await _unitOfWork.CommitAsync(cancellationToken);

            return BaseResponse<Guid>.Success(match.Id, "Match created successfully.", HttpStatusCode.Created);
        }
    }
}
