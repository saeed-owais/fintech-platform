using FinTech.Domain.Constants;
using FluentValidation;

namespace FinTech.Application.Transactions.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryValidator : AbstractValidator<GetTransactionHistoryQuery>
    {
        public GetTransactionHistoryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");


            RuleFor(t => t.TransactionType)
                .Must(type => type == null ||
                      type == TransactionType.Transfer ||
                      type == TransactionType.Withdraw ||
                      type == TransactionType.Deposit)
                .WithMessage("Invalid transaction type");

            RuleFor(x => x.FromDate)
             .LessThanOrEqualTo(DateTime.UtcNow)
             .When(x => x.FromDate.HasValue)
             .WithMessage("From date cannot be in the future");

            RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("To date must be after from date");
        }
    }
}
