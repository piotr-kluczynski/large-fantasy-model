namespace large_fantasy_model.Models.CharacterModels.References
{
    // Class connecting the database Character table and the JSON model for Classes
    public class CharacterClass
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string ClassId { get; set; }
        public int Level { get; set; }
        public string Subtype { get; set; }
    }
}
