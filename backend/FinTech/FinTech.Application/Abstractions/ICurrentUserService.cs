namespace FinTech.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
