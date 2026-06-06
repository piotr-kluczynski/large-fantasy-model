namespace large_fantasy_model.Models.CharacterModels.References
{
    public class CharacterFeature
    {
        public int Id { get; set; }
        public string FeatureName { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
