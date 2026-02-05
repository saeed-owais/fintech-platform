using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Transactions.Commands.Deposit
{
    public record DepositCommand(decimal Amount) : IRequest<Result<DepositResponse>>;
}
