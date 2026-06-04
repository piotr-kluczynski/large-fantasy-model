namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Race
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubType { get; set; }
        public string Size { get; set; }
        public List<Feature> Traits { get; set; }
        public List<Action> Actions { get; set; }
    }
}
