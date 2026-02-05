namespace FinTech.Application.Transactions.Commands.Withdraw
{
    public record WithdrawResponse(
        Guid TransactionId,
        decimal Amount,
        decimal NewBalance,
        DateTime CreatedAt);
}
