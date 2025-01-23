using System.Collections.Generic;

namespace MemoryGame.Business
{
    public interface IHighScoreRepository
    {
        void AddHighScore(HighScore highScore);
        List<HighScore> GetHighScores();

        Task<List<HighScore>> GetHighScoresAsync();
    }
}
