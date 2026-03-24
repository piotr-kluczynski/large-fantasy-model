using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character
{
    // Class describing the treasure the character owns
    public class CharacterTreasure
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
