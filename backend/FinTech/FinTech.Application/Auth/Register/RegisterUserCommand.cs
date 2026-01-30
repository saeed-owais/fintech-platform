using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Auth.Register
{
    public record RegisterUserCommand(string Name, string Email, string Password) : IRequest<Result<RegisterUserResponse>>;
}
