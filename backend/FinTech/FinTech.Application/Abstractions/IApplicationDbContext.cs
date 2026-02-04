using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Wallet> Wallets { get; }
        DbSet<Transaction> Transactions { get; }
        DbSet<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
