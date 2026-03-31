namespace large_fantasy_model.Models.Character.References
{
    // Class connecting the database Character table and the JSON model for Spells
    public class CharacterSpell
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string SpellId { get; set; }
    }
}
