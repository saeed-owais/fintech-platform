using FluentValidation;

namespace FinTech.Application.Transactions.Commands.Withdraw
{
    public class WithdrawValidator : AbstractValidator<WithdrawCommand>
    {
        public WithdrawValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .Must(amount => decimal.Round(amount, 2) == amount)
                .WithMessage("Amount cannot have more than 2 decimal places");
        }
    }
}
