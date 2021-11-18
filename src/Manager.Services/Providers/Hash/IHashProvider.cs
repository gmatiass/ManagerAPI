using Manager.Services.Providers.Hash;

namespace Manager.Services.Interfaces
{
    public interface IHashProvider
    {
        PayloadViewModel GenerateHash(string payload);
        bool VerifyHash(PayloadViewModel payload, string hashed);
    }
}
