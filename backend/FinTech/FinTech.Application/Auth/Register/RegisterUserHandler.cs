using FinTech.Application.Abstractions;
using FinTech.Application.common;
using FinTech.Domain.Constants;
using FinTech.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Auth.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserHandler(IApplicationDbContext context, IPasswordHasher hasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _hasher = hasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exist = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (exist)
            {
                return Result.Failure<RegisterUserResponse>(Error.Conflict("Auth.Register.EmailExists", "Email already exists"));
            }

            var hashedPassword = _hasher.Hash(request.Password);


            var user = new User(
                request.Name,
                request.Email,
                hashedPassword,
                Role.User
                );

            var wallet = new Wallet(user.Id);

            await _context.Users.AddAsync(user, cancellationToken);

            await _context.Wallets.AddAsync(wallet, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);

            return Result.Success(new RegisterUserResponse(token, user.Email, user.Id, user.Role));
        }
    }
}
