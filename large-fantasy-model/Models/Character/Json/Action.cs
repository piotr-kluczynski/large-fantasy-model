namespace large_fantasy_model.Models.Character.Json
{
    public class Action
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public Dice Dice { get; set; }
        public int DamageBonus { get; set; }
        public bool Legendary { get; set; }
        public bool Reaction { get; set; }
    }
}
