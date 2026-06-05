using large_fantasy_model.Models.Compendium;
using System.Text.Json.Serialization;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Race : IRulebookEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("features")]
        public List<string> Features { get; set; } = new();
    }
}
