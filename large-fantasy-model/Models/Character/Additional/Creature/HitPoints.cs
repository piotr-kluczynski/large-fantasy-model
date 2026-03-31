using large_fantasy_model.Models.Character.Json;

namespace large_fantasy_model.Models.Character.Additional.Creature
{
    public class HitPoints
    {
        public int Max { get; set; }
        public int Current { get; set; }
        public int Temporary { get; set; }
        public List<Dice> Dice { get; set; }
    }
}
