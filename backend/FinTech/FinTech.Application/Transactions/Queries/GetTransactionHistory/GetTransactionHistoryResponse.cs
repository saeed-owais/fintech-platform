namespace FinTech.Application.Transactions.Queries.GetTransactionHistory
{
    public record GetTransactionHistoryResponse(
        IEnumerable<TransactionDto> Transactions,
        int TotalCount,
        int PageNumber,
        int PageSize,
        int TotalPages
        );

    public record TransactionDto(
        Guid Id,
        decimal Amount,
        string TransactionType,
        string Status,
        string? SenderEmail, // null for deposits
        string? ReceiverEmail,// null for withdrawals
        DateTime CreatedAt);
}
