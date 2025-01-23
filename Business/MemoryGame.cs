using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MemoryGame.Business
{
    public class MemoryGame
    {
        private readonly List<Card> _cards;
        private readonly IHighScoreRepository _highScoreRepository;
        private int _attempts;
        private DateTime _startTime;

        public readonly int NumberOfCards;

        public MemoryGame(IHighScoreRepository highScoreRepository, int numberOfPairs = 5)
        {
            if (numberOfPairs < 5) {
                throw new ArgumentException("Number of pairs must be at least 5");
            }
            if (numberOfPairs > 14) { // there are only 14 colors in the ConsoleColor enum (excluding black and which)
                throw new ArgumentException("Number of pairs must be at most 14");
            }
            _highScoreRepository = highScoreRepository;
            _cards = InitializeCards(numberOfPairs);
            _attempts = 0;
            _startTime = DateTime.Now;
            NumberOfCards = numberOfPairs * 2;
        }

        private List<Card> InitializeCards(int numberOfPairs)
        {   
            

            var cards = new List<Card>();
            for (int i = 0; i < numberOfPairs; i++)
            {
                ConsoleColor randomColor = (ConsoleColor)i + 1; // Skip the first color (black )
                cards.Add(new Card(i, randomColor));
                cards.Add(new Card(i, randomColor)); // Voeg een paar toe
            }
            return cards.OrderBy(c => Guid.NewGuid()).ToList(); // Schud de kaarten
        }

        public bool FlipCard(int index)
        {
            if (index < 0 || index >= _cards.Count || _cards[index].IsMatched || _cards[index].IsFlipped)
                return false;

            _cards[index].IsFlipped = true;
            return true;
        }

        public bool CheckForMatch(int firstIndex, int secondIndex)
        {
            if (_cards[firstIndex].Id == _cards[secondIndex].Id && firstIndex != secondIndex)
            {
                _cards[firstIndex].IsMatched = true;
                _cards[secondIndex].IsMatched = true;
                return true;
            }
            _cards[firstIndex].IsFlipped = false;
            _cards[secondIndex].IsFlipped = false;
            _attempts++;
            return false;
        }

        public ConsoleColor GetCardColor(int index) => _cards[index].Color;

        public bool GetMatchedStatus(int index) => _cards[index].IsMatched;

        public bool IsGameOver() => _cards.All(c => c.IsMatched);

        public int EndGame()
        {
            int elapsedSeconds = (int)(DateTime.Now - _startTime).TotalSeconds;
            int score = ScoreCalculator.CalculateScore(NumberOfCards, _attempts, elapsedSeconds);
            return score;
        }

        public bool SaveScore(string playerName, int score)
        {
            var topScores = _highScoreRepository.GetHighScores();
            if (topScores.Count < 10 || score > topScores.Min(s => s.Score))
            {
                _highScoreRepository.AddHighScore(new HighScore {Id = Guid.NewGuid(), PlayerName = playerName, Score = score, NumberOfCards = NumberOfCards});
                return true;
            }
            return false;
        }
    }
}
