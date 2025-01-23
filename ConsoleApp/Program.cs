

namespace MemoryGame.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welkom bij het Memory Spel!");
                Console.WriteLine("A. Start spel");
                Console.WriteLine("B. Bekijk highscores");
                Console.WriteLine("C. Verlaat spel");
                Console.Write("Kies een optie: ");
                string input = Console.ReadLine() ?? "";
                Console.Clear();
                switch (input?.ToUpper())
                {
                    case "A":
                        ConsoleGame.Game();
                        break;
                    case "B":
                        ConsoleHighScore.DisplayHighScore();
                        break;
                    case "C":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Ongeldige invoer, probeer opnieuw.");
                        break;
                }
            }
        }
    }
}