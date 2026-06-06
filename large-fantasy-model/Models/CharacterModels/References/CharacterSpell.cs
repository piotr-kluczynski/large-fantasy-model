namespace large_fantasy_model.Models.CharacterModels.References
{
    // Class connecting the database Character table and the JSON model for Spells
    public class CharacterSpell
    {
        public int Id { get; set; }
        public string SpellName { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
