using FinTech.Application.Abstractions;
using FinTech.Application.common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Transactions.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, Result<GetTransactionHistoryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetTransactionHistoryQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<GetTransactionHistoryResponse>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

            if (wallet == null)
                return Result.Failure<GetTransactionHistoryResponse>(Error.NotFound("Wallet.NotFound", "Wallet not found for this user"));

            var query = _context.Transactions
                .Where(t => t.SourceWalletId == wallet.Id || t.DestinationWalletId == wallet.Id);

            if (!string.IsNullOrEmpty(request.TransactionType))
                query = query.Where(t => t.TransactionType == request.TransactionType);

            if (request.FromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= request.ToDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var transactions = await query.OrderByDescending(t => t.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TransactionDto(
                    t.Id,
                    t.Amount,
                    t.TransactionType,
                    t.Status,
                    t.SourceWallet != null ? t.SourceWallet.User.Email : null,
                    t.DestinationWallet != null ? t.DestinationWallet.User.Email : null,
                    t.CreatedAt))
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (decimal)request.PageSize);

            return new GetTransactionHistoryResponse(
                transactions,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages);
        }
    }
}
