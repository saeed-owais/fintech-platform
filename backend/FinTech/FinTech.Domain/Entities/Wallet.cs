namespace FinTech.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public bool IsFrozen { get; private set; }

        private Wallet() { } // For EF

        public Wallet(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            IsFrozen = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Freeze() => IsFrozen = true;

        public void Unfreeze() => IsFrozen = false;
    }

}
