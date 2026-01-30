using FinTech.Application.common;
using MediatR;

namespace FinTech.Application.Auth.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResponse>>;
}
