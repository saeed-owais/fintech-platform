using FinTech.Application.Transactions.Commands.Deposit;
using FinTech.Application.Transactions.Commands.Transfer;
using FinTech.Application.Transactions.Commands.Withdraw;
using FinTech.Application.Transactions.Queries.GetBalance;
using FinTech.Application.Transactions.Queries.GetTransactionHistory;
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

        [HttpGet("balance")]
        public async Task<ActionResult<GetBalanceResponse>> GetBalance(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetBalanceQuery(), cancellation);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }

        [HttpGet("history")]
        public async Task<ActionResult<GetTransactionHistoryResponse>> GetHistory([FromQuery] GetTransactionHistoryQuery query, CancellationToken cancellation)
        {
            var result = await _mediator.Send(query, cancellation);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<DepositResponse>> Deposit([FromBody] DepositCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<WithdrawResponse>> Withdraw([FromBody] WithdrawCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransferResponse>> Transfer([FromBody] TransferCommand command, CancellationToken cancellation)
        {
            var result = await _mediator.Send(command, cancellation);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return CreateProblemDetails(result.Error);
        }
    }
}
