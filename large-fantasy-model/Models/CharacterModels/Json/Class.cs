namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitDie { get; set; }
        public string Spellcasting { get; set; } = ""; 

        public List<Feature> Features { get; set; }
    }
}
