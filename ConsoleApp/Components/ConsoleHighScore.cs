using System;
using MemoryGame.Business;

namespace MemoryGame.ConsoleApp
{
    public static class ConsoleHighScore
    {
        public static void DisplayHighScore()
        {
            var repository = new MemoryGame.DataAccess.HighScoreRepository();
            var game = new MemoryGame.Business.MemoryGame(repository);
            try
            {
                List<HighScore> highScores = game.GetHighScores();

                if (highScores.Count == 0)
                {
                    Console.WriteLine("Er zijn nog geen highscores.");
                    Console.WriteLine("Druk op een toets om terug te gaan.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Highscores:");
                Console.WriteLine("Plaats\tNaam\t\tScore\tKaarten");
                int i = 0;
                foreach (var highScore in highScores)
                {
                    i++;
                    int countLetters = highScore.PlayerName.Count();
                    if (countLetters < 8)
                    {
                        highScore.PlayerName += "\t";
                    }

                    Console.WriteLine(
                        $"{i}\t{highScore.PlayerName}\t{highScore.Score}\t{highScore.NumberOfCards}"
                    );
                }
                Console.WriteLine();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Er is een fout opgetreden bij het ophalen van de highscores.");
                Console.Error.WriteLine(e.Message);
            }

            Console.WriteLine("Druk op een toets om terug te gaan.");
            Console.ReadKey();
        }
    }
}
