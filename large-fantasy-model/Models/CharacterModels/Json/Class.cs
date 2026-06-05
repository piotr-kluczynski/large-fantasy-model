using large_fantasy_model.Models.Compendium;
using System.Text.Json.Serialization;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Class : IRulebookEntity
    {
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("hit_die")]
        public int HitDie { get; set; }
        [JsonPropertyName("spellcasting")]
        public string Spellcasting { get; set; } = ""; 

        // public List<string> Features { get; set; } To be implemented
    }
}
