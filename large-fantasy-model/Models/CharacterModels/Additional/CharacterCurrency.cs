using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.CharacterModels.Additional
{
    // Class describing the treasure the character owns
    public class CharacterCurrency
    {
        public int CharacterId { get; set; }

        public int Copper { get; set; }
        public int Silver { get; set; }
        public int Electrum { get; set; }
        public int Gold { get; set; }
        public int Platinum { get; set; }
    }
}
