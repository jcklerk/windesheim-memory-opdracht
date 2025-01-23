using System.Drawing;

namespace MemoryGame.Business
{
    public class Card
    {
        public int Id { get; }
        public ConsoleColor Color { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFlipped { get; set; }

        public Card(int id, ConsoleColor color)
        {
            Id = id;
            Color = color;
            IsMatched = false;
            IsFlipped = false;
        }
    }
}
