using Manager.Services.Providers.Token;
using System.Threading.Tasks;

namespace Manager.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> CreateSession(string login, string password);
    }
}
