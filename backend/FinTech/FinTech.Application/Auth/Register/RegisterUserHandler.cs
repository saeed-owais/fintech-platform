using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Auth.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
    {
        private readonly IApplicationDbContext _context;

        public RegisterUserHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var exist = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (exist)
            {
                return Result.Failure<RegisterUserResponse>(Error.Conflict("Email", "Email already exists"));
            }
            var user = new User(request.Name, request.Email, request.Password, request.Role);
            var wallet = new Wallet(user.Id);

            await _context.Users.AddAsync(user, cancellationToken);

            await _context.Wallets.AddAsync(wallet, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new RegisterUserResponse(user.Id, user.Email));
        }
    }
}
