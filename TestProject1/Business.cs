using System;
using System.Collections.Generic;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using Moq;
using MemoryGame.Business;

namespace MemoryGame.Tests
{
    [TestFixture]
    public class BusinessTests
    {
        private Mock<IHighScoreRepository> _mockHighScoreRepository;
        private Business.MemoryGame _memoryGame;

        [SetUp]
        public void SetUp()
        {
            _mockHighScoreRepository = new Mock<IHighScoreRepository>();
            _memoryGame = new Business.MemoryGame(_mockHighScoreRepository.Object, numberOfPairs: 5);
        }

        [Test]
        public void Constructor_ShouldInitializeWithCorrectNumberOfCards()
        {
            Assert.That(_memoryGame.NumberOfCards, Is.EqualTo(10)); // 5 pairs, so 10 cards
        }

        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenNumberOfPairsIsBelowMinimum()
        {
            Assert.Throws<ArgumentException>(() => new Business.MemoryGame(_mockHighScoreRepository.Object, 4));
        }

        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenNumberOfPairsExceedsMaximum()
        {
            Assert.Throws<ArgumentException>(() => new Business.MemoryGame(_mockHighScoreRepository.Object, 15));
        }

        [Test]
        public void FlipCard_ShouldReturnFalse_WhenIndexIsOutOfRange()
        {
            Assert.That(_memoryGame.FlipCard(-1), Is.False);
            Assert.That(_memoryGame.FlipCard(10), Is.False); // Out of bounds
        }

        [Test]
        public void FlipCard_ShouldReturnTrue_WhenCardIsSuccessfullyFlipped()
        {
            bool result = _memoryGame.FlipCard(0);
            Assert.That(result, Is.True);
        }


        // [Test]
        // public void IsGameOver_ShouldReturnTrue_WhenAllCardsAreMatched()
        // {
        //     for (int i = 0; i < _memoryGame.NumberOfCards; i += 2)
        //     {
        //         _memoryGame.FlipCard(i);
        //         _memoryGame.FlipCard(i + 1);
        //         _memoryGame.CheckForMatch(i, i + 1);
        //     }
        //     Assert.That(_memoryGame.IsGameOver(), Is.True);
        // }

        [Test]
        public void IsGameOver_ShouldReturnFalse_WhenNotAllCardsAreMatched()
        {
            _memoryGame.FlipCard(0);
            Assert.That(_memoryGame.IsGameOver(), Is.False);
        }

        [Test]
        public void SaveScore_ShouldAddHighScore_WhenScoreIsInTopScores()
        {
            // Arrange
            var highScores = new List<HighScore>
            {
                new HighScore {Score = 50},
                new HighScore {Score = 40}
            };
            _mockHighScoreRepository.Setup(repo => repo.GetHighScores()).Returns(highScores);

            // Act
            bool result = _memoryGame.SaveScore("Player", 60);

            // Assert
            Assert.That(result, Is.True);
            _mockHighScoreRepository.Verify(repo => repo.AddHighScore(It.Is<HighScore>(s => s.Score == 60 && s.PlayerName == "Player")), Times.Once);
        }

        [Test]
        public void SaveScore_ShouldNotAddHighScore_WhenScoreIsNotInTopScores()
        {
            var highScores = new List<HighScore>
            {
                new HighScore {Score = 100},
                new HighScore {Score = 90},
                new HighScore {Score = 80},
                new HighScore {Score = 100},
                new HighScore {Score = 90},
                new HighScore {Score = 80},
                new HighScore {Score = 100},
                new HighScore {Score = 90},
                new HighScore {Score = 80},
                new HighScore {Score = 100},
            };
            _mockHighScoreRepository.Setup(repo => repo.GetHighScores()).Returns(highScores);

            bool result = _memoryGame.SaveScore("Player", 70);

            Assert.That(result, Is.False);
            _mockHighScoreRepository.Verify(repo => repo.AddHighScore(It.IsAny<HighScore>()), Times.Never);
        }
    }
}