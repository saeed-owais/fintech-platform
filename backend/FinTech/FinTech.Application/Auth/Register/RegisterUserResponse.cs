namespace FinTech.Application.Auth.Register
{
    public record RegisterUserResponse(string token, string Email, Guid Id, string Role);

}
