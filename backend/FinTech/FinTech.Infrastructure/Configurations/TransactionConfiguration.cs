using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(t => t.Status)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(t => t.TransactionType)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            builder.HasOne(t => t.SourceWallet)
                   .WithMany(w => w.OutgoingTransactions)
                   .HasForeignKey(t => t.SourceWalletId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            builder.HasOne(t => t.DestinationWallet)
                   .WithMany(w => w.IncomingTransactions)
                   .HasForeignKey(t => t.DestinationWalletId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            builder.HasIndex(t => t.SourceWalletId);
            builder.HasIndex(t => t.DestinationWalletId);
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex(t => t.TransactionType);
            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
