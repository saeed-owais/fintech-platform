using FinTech.Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace FinTech.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string hashPassword, string password)
        {
            var result = _hasher.VerifyHashedPassword(null!, hashPassword, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
