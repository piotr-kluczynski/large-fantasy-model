using System.Text.Json.Serialization;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Weapon : Item
    {
        [JsonPropertyName("damage")]
        public string Damage {  get; set; }
        [JsonPropertyName("range")]
        public string Range { get; set; }
        [JsonPropertyName("throw_range")]
        public string ThrowRange { get; set; }

        // Properties
        [JsonPropertyName("ammunition")]
        public string Ammunition { get; set; }
        [JsonPropertyName("finesse")]
        public bool Finesse { get; set; }
        [JsonPropertyName("heavy")]
        public bool Heavy { get; set; }
        [JsonPropertyName("light")]
        public bool Light { get; set; }
        [JsonPropertyName("loading")]
        public bool Loading { get; set; }
        [JsonPropertyName("monk")]
        public bool Monk { get; set; }
        [JsonPropertyName("reach")]
        public bool Reach { get; set; }
        [JsonPropertyName("thrown")]
        public bool Thrown { get; set; }
        [JsonPropertyName("two_handed")]
        public bool TwoHanded { get; set; }
        [JsonPropertyName("versatile")]
        public bool Versatile { get; set; }
    }
}
