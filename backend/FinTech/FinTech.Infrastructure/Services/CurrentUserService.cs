using FinTech.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinTech.Infrastructure.Services
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId =>
             Guid.TryParse(_httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId)
            ? userId
            : Guid.Empty;
    }
}
