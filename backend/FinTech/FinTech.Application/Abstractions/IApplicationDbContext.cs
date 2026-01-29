using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Wallet> Wallets { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
