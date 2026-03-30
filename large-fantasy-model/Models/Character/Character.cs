using large_fantasy_model.Models.Character.References;
using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Owner { get; set; }

        public string Nickname { get; set; }

        public string Player { get; set; } = "NPC";

        [Range(0, int.MaxValue)]
        public int Xp { get; set; } = 0;

        public CharacterRace Race { get; set; }

        public List<CharacterClass> Classes { get; set; }

        public CharacterBackground Background { get; set; }

        public CharacterDetails Details { get; set; }

        public List<CharacterProficiency> WeaponProficiencies { get; set; }

        public List<CharacterProficiency> ArmorProficiencies { get; set; }

        public List<CharacterProficiency> ToolProficiencies { get; set; }

        public List<CharacterFeat> Feats { get; set; }

        public List<CharacterSpell> Spells { get; set; }

        public List<CharacterWeapon> Weapons { get; set; }

        public List<CharacterEquipment> Equipment { get; set; } // Sprawdź czy wszystko działa poprawnie

        public List<CharacterTreasure> Treasure { get; set; }
    }
}
