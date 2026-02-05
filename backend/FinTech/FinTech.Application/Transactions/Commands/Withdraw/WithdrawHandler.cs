using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Constants;
using FinTech.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Transactions.Commands.Withdraw
{
    public class WithdrawHandler : IRequestHandler<WithdrawCommand, Result<WithdrawResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public WithdrawHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Result<WithdrawResponse>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var wallet = await _context.Wallets.FirstOrDefaultAsync(t => t.UserId == userId);

            if (wallet == null)
                return Result.Failure<WithdrawResponse>(Error.NotFound("Wallet.NotFound", "Wallet not found for this user"));

            if (wallet.IsFrozen)
                return Result.Failure<WithdrawResponse>(Error.Conflict("Wallet.Frozen", "Cannot withdraw from a frozen wallet"));

            var balance = await CalculateBalance(wallet.Id, cancellationToken);

            if (balance < request.Amount)
                return Result.Failure<WithdrawResponse>(Error.Conflict("Withdraw.InsufficientFunds", "Insufficient balance"));

            var transaction = new Transaction(
                request.Amount,
                TransactionStatus.Completed,
                TransactionType.Withdraw,
                wallet.Id,
                null);

            await _context.Transactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var newBalance = await CalculateBalance(wallet.Id, cancellationToken);

            return new WithdrawResponse(transaction.Id, transaction.Amount, newBalance, transaction.CreatedAt);
        }

        private async Task<decimal> CalculateBalance(Guid walletId, CancellationToken cancellationToken)
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
                    balance += t.Amount;

                // Money going OUT from this wallet
                if (t.SourceWalletId == walletId)
                    balance -= t.Amount;
            }

            return balance;
        }
    }
}
