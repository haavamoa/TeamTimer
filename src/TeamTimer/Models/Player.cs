namespace TeamTimer.Models
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public long PlayTime { get; set; }
    }
}