namespace MemoryGame.Business
{
    public class HighScore
    {
        public Guid Id { get; set; }
        public string PlayerName { get; set; }
        public int Score { get; set; }

        public int NumberOfCards { get; set; }
    }
}
