using MemoryGame.Business;
using Microsoft.EntityFrameworkCore;

namespace MemoryGame.DataAccess
{
    public class HighScoreDbContext : DbContext
    {
        public DbSet<HighScore> HighScores { get; set; }

        public HighScoreDbContext()
        {
            Initialize();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                string packageFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CS.MemoryGame.JcklerkDev"
                ); // dev.jcklerk.MemoryApp
                Console.WriteLine("Database path:");
                Console.WriteLine(packageFolder);
                Console.WriteLine("Database path end");

                if (!Directory.Exists(packageFolder))
                {
                    Directory.CreateDirectory(packageFolder);
                }

                var dbPath = Path.Combine(packageFolder, "highscores.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
            catch (Exception e)
            {
                throw new Exception("Could not create database file", e);
            }
            // optionsBuilder.UseSqlite("Data Source=highscores.db");
        }

        public void Initialize()
        {
            // Ensure the database is created if it doesn’t exist
            Database.EnsureCreated();

            // Create the table manually if it doesn't exist
            var tableExists = Database.ExecuteSqlRaw(
                "SELECT name FROM sqlite_master WHERE type='table' AND name='HighScores';"
            );

            if (string.IsNullOrEmpty(tableExists.ToString()))
            {
                Database.ExecuteSqlRaw(
                    @"
                    CREATE TABLE HighScores (
                        Id INT PRIMARY KEY AUTOINCREMENT,
                        Score INT NOT NULL,
                        PlayerName TINYTEXT NOT NULL,
                        NumberOfCards INT NOT NULL,
                    );"
                );
            }
        }
    }
}
