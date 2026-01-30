using FinTech.Application.Abstractions;
using FinTech.Application.common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Auth.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserHandler(IApplicationDbContext context, IPasswordHasher hasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _hasher = hasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }


        public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null)
            {
                var error = Error.NotFound("Auth.Login", "Invalid email or password");
                return Result.Failure<LoginUserResponse>(error);
            }

            // check password
            bool validPassword = _hasher.Verify(user.PasswordHash, request.Password);

            if (!validPassword)
            {
                var error = Error.NotFound("Auth.Login", "Invalid email or password");
                return Result.Failure<LoginUserResponse>(error);
            }

            //generate token
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);

            return Result.Success(new LoginUserResponse(token, user.Name, user.Email, user.Role));

        }
    }
}
