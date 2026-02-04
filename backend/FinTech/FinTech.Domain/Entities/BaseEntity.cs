namespace FinTech.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

    }
}
