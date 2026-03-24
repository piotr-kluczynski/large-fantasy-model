namespace large_fantasy_model.Models.Character.References
{
    // Class connecting the database Character table and the JSON model for Classes
    public class CharacterClass
    {
        public int CharacterId { get; set; }
        public string ClassName { get; set; }
    }
}
