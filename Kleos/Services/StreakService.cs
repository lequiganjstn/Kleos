using Kleos.Data;
using Kleos.Models;
using System;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public class StreakService : IStreakService
    {
        private readonly DatabaseService _db;

        public StreakService(DatabaseService db)
        {
            _db = db;
        }

        public async Task<Streak> GetStreakAsync(int userId)
        {
            var streak = await _db.GetStreakAsync(userId);
            if (streak is null)
            {
                streak = new Streak { UserId = userId };
                await _db.SaveStreakAsync(streak);
            }
            return streak;
        }

        public async Task RecordActivityAsync(int userId)
        {
            var streak = await GetStreakAsync(userId);
            var today = DateTime.Today;

            if (streak.LastActivityDate.Date == today)
            {
                streak.TotalTasksCompleted++;
            }
            else if (streak.LastActivityDate.Date == today.AddDays(-1))
            {
                streak.CurrentStreak++;
                streak.TotalTasksCompleted++;
                streak.LastActivityDate = DateTime.Now;
                if (streak.CurrentStreak > streak.LongestStreak)
                    streak.LongestStreak = streak.CurrentStreak;
            }
            else
            {
                streak.CurrentStreak = 1;
                streak.TotalTasksCompleted++;
                streak.LastActivityDate = DateTime.Now;
                if (streak.LongestStreak == 0)
                    streak.LongestStreak = 1;
            }

            await _db.SaveStreakAsync(streak);
        }
    }
}