using Manager.Services.Providers.Hash;

namespace Manager.Services.Interfaces
{
    public interface IHashProvider
    {
        PayloadModel GenerateHash(string payload);
        bool VerifyHash(PayloadModel payload, string hashed);
    }
}
