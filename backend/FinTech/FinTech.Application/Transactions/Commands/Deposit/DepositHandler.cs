using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Constants;
using FinTech.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Transactions.Commands.Deposit
{
    public class DepositHandler : IRequestHandler<DepositCommand, Result<DepositResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DepositHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<DepositResponse>> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            // Get user's wallet
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

            if (wallet is null)
            {
                return Result.Failure<DepositResponse>(
                    Error.NotFound("Wallet.NotFound", "Wallet not found for this user"));
            }

            // Check if wallet is frozen
            if (wallet.IsFrozen)
            {
                return Result.Failure<DepositResponse>(
                    Error.Conflict("Wallet.Frozen", "Cannot deposit to a frozen wallet"));
            }

            // Create deposit transaction
            var transaction = new Transaction(
                request.Amount,
                TransactionStatus.Completed,
                TransactionType.Deposit,
                null,        // No source wallet (money comes from outside)
                wallet.Id    // Destination is user's wallet
            );

            await _context.Transactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Calculate new balance
            var newBalance = await CalculateBalanceAsync(wallet.Id, cancellationToken);

            return new DepositResponse(
                transaction.Id,
                request.Amount,
                newBalance,
                transaction.CreatedAt);
        }

        private async Task<decimal> CalculateBalanceAsync(Guid walletId, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed)
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                .ToListAsync(cancellationToken);

            decimal balance = 0;

            foreach (var t in transactions)
            {
                // Money coming IN to this wallet
                if (t.DestinationWalletId == walletId)
                {
                    balance += t.Amount;
                }
                // Money going OUT from this wallet
                if (t.SourceWalletId == walletId)
                {
                    balance -= t.Amount;
                }
            }

            return balance;
        }
    }
}
