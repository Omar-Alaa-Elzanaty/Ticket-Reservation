using Core.Dtos;
using Core.Features.Auth.Login;
using Core.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<LoginQueryDto>>> Login([FromBody] LoginQuery query)
        {
            return (await _mediator.Send(query));
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<bool>>> Register([FromBody] Registercommand command)
        {
            return (await _mediator.Send(command));
        }
    }
}
