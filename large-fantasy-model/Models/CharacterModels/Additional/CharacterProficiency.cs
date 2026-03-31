namespace large_fantasy_model.Models.CharacterModels.Additional
{
    // Class representing different character proficiencies (Weapon, Armor, Tool) in the database
    public class CharacterProficiency
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public ProficiencyType Type { get; set; }

        public Character Character { get; set; }
    }

    public enum ProficiencyType
    {
        Weapon,
        Armor,
        Tool
    }
}
