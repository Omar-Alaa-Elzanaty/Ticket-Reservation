using Core.Dtos;
using Core.Models;
using Core.Ports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RedLockNet;
using System.Net;

namespace Core.Features.Tickets.Commands.Reserve
{
    public class TicketReservationCommand : IRequest<BaseResponse<int>>
    {
        public int TicketId { get; set; }
    }

    internal class TicketReservationCommandHandler : IRequestHandler<TicketReservationCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDistributedLockFactory _distributedLockFactory;

        public TicketReservationCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext,
            IDistributedLockFactory distributedLockFactory)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContext = httpContext;
            _distributedLockFactory = distributedLockFactory;
        }
        public async Task<BaseResponse<int>> Handle(TicketReservationCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

            string resource = $"lock:ticket:{command.TicketId}";
            var expiry = TimeSpan.FromSeconds(30);
            var wait = TimeSpan.FromSeconds(10);
            var retry = TimeSpan.FromSeconds(1);

            await using (var readLock = await _distributedLockFactory.CreateLockAsync(resource, expiry, wait, retry, cancellationToken))
            {
                if (!readLock.IsAcquired)
                {
                    return BaseResponse<int>.Failure("Ticket is hold.", HttpStatusCode.RequestTimeout);
                }

                if (!await _unitOfWork.TicketRepo.ReserveTicket(command.TicketId, user.Id))
                {
                    return BaseResponse<int>.Failure("Ticket is already reserved", HttpStatusCode.Conflict);
                }
            }
            return BaseResponse<int>.Success("Ticket reserved.");
        }
    }
}
