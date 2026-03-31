namespace large_fantasy_model.Models.CharacterModels.Additional.Creature
{
    public class HitPoints
    {
        public int Max { get; set; }
        public int Current { get; set; }
        public int Temporary { get; set; }
        public List<Dice> Dice { get; set; }
    }
}
