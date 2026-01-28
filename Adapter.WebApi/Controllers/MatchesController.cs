using Adapter.SqlServer.Repository;
using Core.Dtos;
using Core.Features.Matches.Commands.Create;
using Core.Features.Tickets.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MatchesController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Guid>>> CreateMatch(CreateMatchCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("ticket")]
        public async Task<ActionResult<BaseResponse<Guid>>> CreateMatchTicket(CreateTicketCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
