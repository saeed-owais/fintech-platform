using FinTech.Application.Transactions.Commands.Deposit;
using FinTech.Application.Transactions.Commands.Withdraw;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Authorize]
    public class TransactionController : BaseController
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("deposit")]
        public async Task<ActionResult<DepositResponse>> Deposit(DepositCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }
        [HttpPost("withdraw")]
        public async Task<ActionResult<DepositResponse>> Withdraw(WithdrawCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }
    }
}
