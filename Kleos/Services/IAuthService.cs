using Kleos.Models;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password);
        Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password);
        void SetCurrentUser(User user);
        User? GetCurrentUser();
        void Logout();
        bool IsLoggedIn { get; }
    }
}