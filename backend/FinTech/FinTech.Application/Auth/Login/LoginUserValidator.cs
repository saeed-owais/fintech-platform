using FluentValidation;

namespace FinTech.Application.Auth.Login
{
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid Email");

            RuleFor(u => u.Password)
                .NotEmpty();
        }
    }
}
