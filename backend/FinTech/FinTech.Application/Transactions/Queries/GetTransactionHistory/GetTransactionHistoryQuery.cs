using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Transactions.Queries.GetTransactionHistory
{
    public record GetTransactionHistoryQuery(
        string? TransactionType,  // "Deposit", "Withdraw", "Transfer", or null for all
        DateTime? FromDate,
        DateTime? ToDate,
        int PageNumber = 1,
        int PageSize = 10
        ) : IRequest<Result<GetTransactionHistoryResponse>>;
}
