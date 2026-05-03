using Kleos.Models;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public interface IStreakService
    {
        Task<Streak> GetStreakAsync(int userId);
        Task RecordActivityAsync(int userId);
    }
}