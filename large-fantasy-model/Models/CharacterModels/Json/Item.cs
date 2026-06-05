using large_fantasy_model.Models.Compendium;
using System.Text.Json.Serialization;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Item : IRulebookEntity
    {
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("weight")]
        public int Weight { get; set; }
        [JsonPropertyName("magic")]
        public bool Magic { get; set; }
    }
}
