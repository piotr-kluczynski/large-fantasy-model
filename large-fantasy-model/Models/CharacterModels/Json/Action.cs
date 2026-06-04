namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Action
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public string Dice { get; set; }
        public int DamageBonus { get; set; }
        public bool Legendary { get; set; }
        public bool Reaction { get; set; }
    }
}
