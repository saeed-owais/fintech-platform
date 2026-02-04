using FinTech.Application.Abstractions;
using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            UpdateAuditableEntities();
            return base.SaveChanges();
        }
        private void UpdateAuditableEntities()
        {
            var entityEntries = ChangeTracker.Entries<BaseEntity>();
            var userId = _currentUserService.UserId;

            foreach (var entityEntry in entityEntries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.Entity.CreatedBy = userId;
                        entityEntry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entityEntry.Entity.ModifiedBy = userId;
                        entityEntry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entityEntry.Entity.IsDeleted = true;
                        entityEntry.State = EntityState.Modified;
                        entityEntry.Entity.ModifiedBy = userId;
                        entityEntry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
