using large_fantasy_model.Models.Character.Json;

namespace large_fantasy_model.Models.Character
{
    // Specjalny przypadek klasy, która jednocześnie jest tabelą dodającą kontent do tabeli Character (pole Quantity - liczba przedmiotów), a także referencją do modelu "Item", który odpowiada plikom JSON
    public class CharacterEquipment
    {
        public int CharacterId { get; set; }
        public string ItemName { get; set; }

        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
