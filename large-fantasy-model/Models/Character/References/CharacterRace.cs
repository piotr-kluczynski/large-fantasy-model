namespace large_fantasy_model.Models.Character.References
{
    // Class connecting the database Character table and the JSON model for Races
    public class CharacterRace
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int RaceId { get; set; }
    }
}
