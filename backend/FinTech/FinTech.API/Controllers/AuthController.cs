using FinTech.Application.Auth.Login;
using FinTech.Application.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    public class AuthController : BaseController
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

            return CreateProblemDetails(result.Error);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess) return Ok(result.Value);

            return CreateProblemDetails(result.Error);

        }
    }
}
