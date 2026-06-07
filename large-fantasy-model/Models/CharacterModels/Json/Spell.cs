using large_fantasy_model.Models.Compendium;
using System.Text.Json.Serialization;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Spell : IRulebookEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("higher_level")]
        public string HigherLevel { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("casting_time")]
        public string CastingTime { get; set; }

        [JsonPropertyName("range_area")]
        public string RangeArea { get; set; }

        [JsonPropertyName("component_v")]
        public bool ComponentVerbal { get; set; }
        [JsonPropertyName("component_s")]
        public bool ComponentSomatic { get; set; }
        [JsonPropertyName("components_m")]
        public string ComponentMaterials { get; set; }

        [JsonPropertyName("ritual")]
        public bool Ritual { get; set; }

        [JsonPropertyName("concentration")]
        public bool Concentration { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("school")]
        public string School { get; set; }

        [JsonPropertyName("attack_save")]
        public string AttackSave { get; set; }

        [JsonPropertyName("damage_effect")]
        public string DamageEffect { get; set; }
    }
}
