namespace FinTech.Application.Transactions.Commands.Deposit
{
    public record DepositResponse(
        Guid TransactionId,
        decimal Amount,
        decimal NewBalance,
        DateTime CreatedAt);
}
