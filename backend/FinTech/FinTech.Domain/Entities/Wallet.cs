namespace FinTech.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public bool IsFrozen { get; private set; }
        public ICollection<Transaction> OutgoingTransactions { get; private set; } = new List<Transaction>();
        public ICollection<Transaction> IncomingTransactions { get; private set; } = new List<Transaction>();

        public User User { get; set; }
        private Wallet() { } // For EF

        public Wallet(Guid userId)
        {
            UserId = userId;
            IsFrozen = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Freeze() => IsFrozen = true;

        public void Unfreeze() => IsFrozen = false;
    }

}
