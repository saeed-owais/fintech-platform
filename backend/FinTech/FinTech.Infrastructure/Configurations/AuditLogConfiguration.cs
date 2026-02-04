using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Configurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.Details)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(a => a.UserId)
                   .IsRequired();

            builder.Property(a => a.Timestamp)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.Timestamp);
            builder.HasIndex(a => a.Action);
        }
    }
}
