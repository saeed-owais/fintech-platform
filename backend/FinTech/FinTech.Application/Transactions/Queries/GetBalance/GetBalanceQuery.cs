using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Transactions.Queries.GetBalance
{
    public record GetBalanceQuery() : IRequest<Result<GetBalanceResponse>>;
}
