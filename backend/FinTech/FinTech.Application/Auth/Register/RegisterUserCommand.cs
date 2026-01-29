using FinTech.Application.common;
using FinTech.Domain.Enums;
using MediatR;

namespace FinTech.Application.Auth.Register
{
    public record RegisterUserCommand(string Name, string Email, string Password, Role Role) : IRequest<Result<RegisterUserResponse>>;
}
