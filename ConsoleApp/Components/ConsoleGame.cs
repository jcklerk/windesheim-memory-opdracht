using System;

namespace MemoryGame.ConsoleApp
{
    public static class ConsoleGame
    {
        public static void Game()
        {
            try
            {
                Console.WriteLine("Met hoeveel setjes kaarten wil je spelen? (min: 5 max: 14)");
                int amountOfPairConsole= int.Parse(Console.ReadLine() ?? "5"); // ?? "5" is a default value in case the user enters nothing
                
                int amountOfPairs = (amountOfPairConsole >= 5 && amountOfPairConsole <= 14) ? amountOfPairConsole : 5;
                Console.WriteLine("Je hebt gekozen voor " + amountOfPairs + " setjes kaarten. Je hebt dus " + amountOfPairs * 2 + " kaarten in totaal.");
                Console.WriteLine("Druk op een toets om te starten.");
                Console.ReadKey();
                var repository = new MemoryGame.DataAccess.HighScoreRepository();
                var game = new MemoryGame.Business.MemoryGame(repository, amountOfPairs);
                Console.Clear();
                Console.WriteLine("Memory Spel Start!");
                while (!game.IsGameOver())
                {
                    DisplayConsoleCard(game, null, null);
                    try
                    {
                        Console.Write("Voer eerste kaart index in: ");
                        int firstIndex = int.Parse(Console.ReadLine() ?? "0"); // ?? "0" is a default value in case the user enters nothing
                        game.FlipCard(firstIndex);
                        Console.Clear();
                        DisplayConsoleCard(game, firstIndex, null);
                        Console.Write("Voer tweede kaart index in: ");
                        int secondIndex = int.Parse(Console.ReadLine() ?? "0");  // ?? "0" is a default value in case the user enters nothing
                        game.FlipCard(secondIndex);
                        Console.Clear();
                        bool match = game.CheckForMatch(firstIndex, secondIndex);
                        DisplayConsoleCard(game, firstIndex, secondIndex);
                        if (match)
                            Console.WriteLine("Match gevonden!");
                        else
                            Console.WriteLine("Geen match, probeer opnieuw.");
                        
                        Thread.Sleep(500);
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Ongeldige invoer, probeer opnieuw.");
                        Thread.Sleep(500);
                    }
                    Console.Clear();
                }

                int score = game.EndGame();
                Console.Write("Voer je naam in voor de highscore: ");
                string name = Console.ReadLine() ?? "Anoniem"; // ?? "Anoniem" is a default value in case the user enters nothing
                bool HighScore = game.SaveScore(name, score);
                if (HighScore) {

                
                    Console.WriteLine("New highscore!");
                    Console.WriteLine("Youre score is: " + score);
                    Console.WriteLine();
                    ConsoleHighScore.DisplayHighScore();
                    }
                else {
                    Console.WriteLine("Geen highscore.");
                    Console.WriteLine("Druk op een toets om terug te gaan naar menu.");
                    Console.ReadKey();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Er is een fout opgetreden, probeer opnieuw.");
                Console.Error.WriteLine(e.ToString());
                Thread.Sleep(1000);
            }
        }

        private static void DisplayConsoleCard(MemoryGame.Business.MemoryGame game, int? firstIndex, int? secondIndex)
        {
            for (int i = 0; i < game.NumberOfCards; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (game.GetMatchedStatus(i))
                {
                    Console.ForegroundColor = game.GetCardColor(i);
                    Console.Write("M ");
                }
                else
                if ((i == firstIndex || i == secondIndex) && (firstIndex != null || secondIndex != null))
                {
                    Console.ForegroundColor = game.GetCardColor(i);
                    Console.Write("X ");
                }
                else
                    Console.Write(i + " ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

        }
    }
}
