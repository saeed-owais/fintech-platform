using FluentValidation;

namespace FinTech.Application.Auth.Register
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8).WithMessage("at least 8 char..")
                .MaximumLength(10).WithMessage("max length 10 char..");

            RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Name must be at least 3 char..")
                .MaximumLength(50).WithMessage("Name must be at least 50 char..");
        }
    }
}
