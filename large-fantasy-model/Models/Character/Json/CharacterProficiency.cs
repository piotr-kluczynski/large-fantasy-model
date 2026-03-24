namespace large_fantasy_model.Models.Character.Json
{
    // Class representing different character proficiencies (Weapon, Armor, Tool) in the database
    public class CharacterProficiency
    {
        public int Id { get; set; }
        public int Character { get; set; }
        public string Name { get; set; }
    }
}
