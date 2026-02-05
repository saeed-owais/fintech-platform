using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Constants;
using FinTech.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Transactions.Commands.Transfer
{
    public class TransferHandler : IRequestHandler<TransferCommand, Result<TransferResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public TransferHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Result<TransferResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var senderId = _currentUserService.UserId;
            var senderEmail = _currentUserService.Email ?? "Unknown";

            var senderWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == senderId, cancellationToken);

            if (senderWallet == null)
                return Result.Failure<TransferResponse>(Error.NotFound("Wallet.NotFound", "Sender wallet not found"));

            if (senderWallet.IsFrozen)
                return Result.Failure<TransferResponse>(Error.Conflict("Wallet.Frozen", "Cannot transfer from a frozen wallet"));

            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.ReceiverEmail, cancellationToken);

            if (receiver == null)
                return Result.Failure<TransferResponse>(Error.NotFound("Receiver.NotFound", "Receiver not found"));

            if (senderId == receiver.Id)
                return Result.Failure<TransferResponse>(Error.Conflict("Transfer.SelfTransfer", "Cannot transfer to yourself"));

            var receiverWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == receiver.Id, cancellationToken);

            if (receiverWallet == null)
                return Result.Failure<TransferResponse>(Error.NotFound("Wallet.NotFound", "Receiver wallet not found"));

            if (receiverWallet.IsFrozen)
                return Result.Failure<TransferResponse>(Error.Conflict("Wallet.Frozen", "Cannot transfer to a frozen wallet"));

            var senderBalance = await calculateBalance(senderWallet.Id, cancellationToken);

            if (senderBalance < request.Amount)
                return Result.Failure<TransferResponse>(Error.Conflict("Transfer.InsufficientFunds", "Insufficient balance"));

            var transaction = new Transaction(
                request.Amount,
                TransactionStatus.Completed,
                TransactionType.Transfer,
                senderWallet.Id,
                receiverWallet.Id);


            await _context.Transactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var newBalance = await calculateBalance(senderWallet.Id, cancellationToken);

            return new TransferResponse(
                transaction.Id,
                senderEmail,
                request.ReceiverEmail,
                request.Amount,
                newBalance,
                transaction.CreatedAt
                );
        }

        private async Task<decimal> calculateBalance(Guid walletId, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                .Where(t => t.Status == TransactionStatus.Completed)
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
