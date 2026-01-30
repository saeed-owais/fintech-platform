namespace FinTech.Application.Abstractions
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string hashPassword, string password);
    }
}
