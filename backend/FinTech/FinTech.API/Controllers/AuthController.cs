using FinTech.Application.Auth.Login;
using FinTech.Application.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess) return Ok(result.Value);

            else return BadRequest(result.Error);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);

        }
    }
}
