using FinTech.Domain.Enums;

namespace FinTech.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public Role Role { get; private set; }

        private User() { } // For EF

        public User(string name, string email, string passwordHash, Role role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
