namespace Manager.Services.Providers.Token
{
    public interface ITokenGenerator
    {
        string GenerateToken(string login);
    }
}
