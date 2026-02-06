using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Transactions.Queries.GetBalance
{
    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, Result<GetBalanceResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetBalanceQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Result<GetBalanceResponse>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return Result.Failure<GetBalanceResponse>(Error.NotFound("Wallet.NotFound", "Wallet not found for this user"));

            var balance = await CalculateBalanceAsync(wallet.Id, cancellationToken);

            return new GetBalanceResponse(balance, wallet.Id, wallet.IsFrozen, wallet.ModifiedAt);
        }

        private async Task<decimal> CalculateBalanceAsync(Guid walletId, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed)
                .Where(t => t.DestinationWalletId == walletId || t.SourceWalletId == walletId)
                .ToListAsync(cancellationToken);

            decimal balance = 0;
            foreach (var t in transactions)
            {
                if (t.DestinationWalletId == walletId)
                    balance += t.Amount;

                if (t.SourceWalletId == walletId)
                    balance -= t.Amount;
            }

            return balance;
        }
    }
}
