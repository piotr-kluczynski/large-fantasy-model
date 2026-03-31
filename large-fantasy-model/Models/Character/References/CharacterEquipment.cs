using large_fantasy_model.Models.Character.Json;

namespace large_fantasy_model.Models.Character.References
{    public class CharacterEquipment
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
