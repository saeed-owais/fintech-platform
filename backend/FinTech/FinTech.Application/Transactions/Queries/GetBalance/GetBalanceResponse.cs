namespace FinTech.Application.Transactions.Queries.GetBalance
{
    public record GetBalanceResponse(
        decimal Balance,
        Guid WalletId,
        bool IsFrozen,
        DateTime LastUpdated
        );
}
