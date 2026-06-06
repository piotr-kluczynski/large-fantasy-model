namespace large_fantasy_model.Models.CharacterModels.Additional
{
    // Class describing the character background in the database
    public class CharacterBackground
    {
        public int Id { get; set; }
        public string BackgroundName { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
