namespace FinTech.Application.Transactions.Commands.Transfer
{
    public record TransferResponse(
        Guid TransactionId,
        string SenderEmail,
        string ReceiverEmail,
        decimal Amount,
        decimal NewBalance,
        DateTime CreatedAt
        );
}
