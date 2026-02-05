using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Transactions.Commands.Withdraw
{
    public record WithdrawCommand(decimal Amount) : IRequest<Result<WithdrawResponse>>;
}
