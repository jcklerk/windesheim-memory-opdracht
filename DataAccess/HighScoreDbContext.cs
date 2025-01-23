using Microsoft.EntityFrameworkCore;
using MemoryGame.Business;

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
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "highscores.db");
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
                "SELECT name FROM sqlite_master WHERE type='table' AND name='HighScores';");

            if (string.IsNullOrEmpty(tableExists.ToString()))
            {
                Database.ExecuteSqlRaw(@"
                    CREATE TABLE HighScores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Score INTEGER NOT NULL,
                        PlayerName TEXT NOT NULL,
                        NumberOfCards INTEGER NOT NULL,
                    );");
            }
        }
    }
}
