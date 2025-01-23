using System.Collections.Generic;
using System.Linq;
using MemoryGame.Business;
using MemoryGame.DataAccess;
using NUnit.Framework;

namespace MemoryGame.Tests
{
    [TestFixture]
    public class DataAccessTests
    {
        private HighScoreRepository _repository;

        [SetUp]
        public void SetUp()
        {
            // Initialize the repository and ensure a fresh database for each test
            _repository = new HighScoreRepository();
            using var context = new HighScoreDbContext();
            context.Database.EnsureDeleted();
            context.Initialize();
        }

        [Test]
        public void AddHighScore_ShouldAddScoreToDatabase()
        {
            // Arrange
            var highScore = new HighScore { Score = 100, PlayerName = "Player1" };

            // Act
            _repository.AddHighScore(highScore);
            var result = _repository.GetHighScores();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].PlayerName, Is.EqualTo("Player1"));
            Assert.That(result[0].Score, Is.EqualTo(100));
        }

        [Test]
        public void AddHighScore_ShouldLimitToTop10Scores()
        {
            // Arrange
            for (int i = 1; i <= 15; i++)
            {
                _repository.AddHighScore(new HighScore { Score = i * 10, PlayerName = $"Player{i}" });
            }

            // Act
            var result = _repository.GetHighScores();

            // Assert
            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.Max(x => x.Score), Is.EqualTo(150));
            Assert.That(result.Min(x => x.Score), Is.EqualTo(60));
        }

        [Test]
        public void AddHighScore_ShouldRemoveLowestScoreWhenMoreThan10()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                _repository.AddHighScore(new HighScore { Score = i * 10, PlayerName = $"Player{i}" });
            }

            // Act
            _repository.AddHighScore(new HighScore { Score = 200, PlayerName = "NewTopPlayer" });
            var result = _repository.GetHighScores();

            // Assert
            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.Any(h => h.Score == 10), Is.False, "The lowest score should have been removed.");
            Assert.That(result.Any(h => h.Score == 200), Is.True, "The new high score should be in the list.");
        }

        [Test]
        public void GetHighScores_ShouldReturnScoresInDescendingOrder()
        {
            // Arrange
            _repository.AddHighScore(new HighScore { Score = 50, PlayerName = "Player1" });
            _repository.AddHighScore(new HighScore { Score = 100, PlayerName = "Player2" });
            _repository.AddHighScore(new HighScore { Score = 75, PlayerName = "Player3" });

            // Act
            var result = _repository.GetHighScores();

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Score, Is.EqualTo(100));
            Assert.That(result[1].Score, Is.EqualTo(75));
            Assert.That(result[2].Score, Is.EqualTo(50));
        }
    }
}