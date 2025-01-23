using MemoryGame.Business;
using Microsoft.EntityFrameworkCore;

namespace MemoryGame.DataAccess
{
    public class HighScoreRepository : IHighScoreRepository
    {
        public void AddHighScore(HighScore highScore)
        {
            using var context = new HighScoreDbContext();
            context.Initialize();
            context.HighScores.Add(highScore);
            if (context.HighScores.Count() > 10)
            {
                // Remove the lowest score if there are more than 10 high scores
                context.HighScores.Remove(context.HighScores.OrderBy(h => h.Score).First());
            }
            context.SaveChanges();
        }

        public List<HighScore> GetHighScores()
        {
            using var context = new HighScoreDbContext();
            context.Initialize();
            return context.HighScores
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToList();
        }

        public async Task<List<HighScore>> GetHighScoresAsync()
        {
            using var context = new HighScoreDbContext();
            context.Initialize();
            return await context.HighScores
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToListAsync(); // Use ToListAsync for true async execution
        }
    }
}
