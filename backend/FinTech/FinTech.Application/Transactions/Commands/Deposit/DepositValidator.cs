using FluentValidation;

namespace FinTech.Application.Transactions.Commands.Deposit
{
    public class DepositValidator : AbstractValidator<DepositCommand>
    {
        public DepositValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .Must(amount => decimal.Round(amount, 2) == amount)
                .WithMessage("Amount cannot have more than 2 decimal places");
        }
    }
}
