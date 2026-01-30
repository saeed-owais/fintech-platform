namespace FinTech.Application.Auth.Login
{
    public record LoginUserResponse(string Token, string Name, string Email, string Role);
}
