using FluentValidation;

namespace FinTech.Application.Transactions.Commands.Transfer
{
    public class TransferValidator : AbstractValidator<TransferCommand>
    {
        public TransferValidator()
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .Must(amount => decimal.Round(amount, 2) == amount)
                .WithMessage("Amount cannot have more than 2 decimal places");

            RuleFor(t => t.ReceiverEmail).NotEmpty().EmailAddress();
        }
    }
}
