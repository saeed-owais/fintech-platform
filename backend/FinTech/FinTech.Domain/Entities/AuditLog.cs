namespace FinTech.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public string Action { get; private set; }
        public string Details { get; private set; }

        public Guid UserId { get; private set; }
        public DateTime Timestamp { get; private set; }

        public AuditLog(Guid userId, string action, string details)
        {
            UserId = userId;
            Action = action;
            Details = details;
            Timestamp = DateTime.UtcNow;
        }

        // For EF Core
        private AuditLog() { }

    }
}
