using System;

namespace MemoryGame.Business
{
    public static class ScoreCalculator
    {
        public static int CalculateScore(int numCards, int attempts, int seconds)
        {
            // ((Aantal kaarten)2 / (Tijd in seconden * aantal pogingen)) * 1000
            double score = ((numCards * numCards) / (double)(attempts * seconds)) * 1000;
            return (int)score;
        }
    }
}
