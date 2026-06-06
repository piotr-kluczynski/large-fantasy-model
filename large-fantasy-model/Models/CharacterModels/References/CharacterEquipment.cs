using large_fantasy_model.Models.CharacterModels.Json;

namespace large_fantasy_model.Models.CharacterModels.References
{    public class CharacterEquipment
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
