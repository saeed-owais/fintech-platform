using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Configurations
{
    internal class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId)
                   .IsRequired();

            builder.HasIndex(w => w.UserId)
                   .IsUnique();

            builder.Property(w => w.IsFrozen)
                   .IsRequired();

            builder.Property(w => w.CreatedAt)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithOne()
                   .HasForeignKey<Wallet>(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(w => !w.IsDeleted);
        }
    }
}
