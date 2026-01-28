using Core.Dtos;
using Core.Features.Tickets.Commands.Reserve;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ReserveTicket")]
        public async Task<ActionResult<BaseResponse<bool>>> ReserveTicket([FromBody] TicketReservationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
