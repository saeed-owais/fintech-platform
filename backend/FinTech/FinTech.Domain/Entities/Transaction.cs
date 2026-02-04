
using FinTech.Domain.Constants;

namespace FinTech.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; private set; }
        public string Status { get; private set; }
        public string TransactionType { get; private set; }
        public Guid? SourceWalletId { get; private set; }
        public Guid? DestinationWalletId { get; private set; }
        public Wallet? SourceWallet { get; set; }
        public Wallet? DestinationWallet { get; set; }

        public Transaction() { }

        public Transaction(
            decimal amount,
            string status,
            string transactionType,
            Guid sourceWalletId,
            Guid destinationWalletId)
        {
            Amount = amount;
            Status = status;
            TransactionType = transactionType;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
        }

        public void MarkCompleted() => Status = TransactionStatus.Completed;
        public void MarkFailed() => Status = TransactionStatus.Failed;
    }
}
