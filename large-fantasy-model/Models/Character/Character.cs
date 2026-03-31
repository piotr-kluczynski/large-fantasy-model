using large_fantasy_model.Models.Character.Additional;
using large_fantasy_model.Models.Character.Additional.Creature;
using large_fantasy_model.Models.Character.References;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models.Character
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Owner { get; set; }

        public Creature Creature { get; set; }

        public string Nickname { get; set; }

        public string Player { get; set; } = "NPC";

        [Range(0, int.MaxValue)]
        public int Xp { get; set; } = 0;

        public CharacterRace Race { get; set; }

        public List<CharacterClass> Classes { get; set; }

        public CharacterBackground Background { get; set; }

        public CharacterDetails Details { get; set; }

        public List<CharacterProficiency> Proficiencies { get; set; }

        [NotMapped]
        public List<CharacterProficiency> WeaponProficiencies => 
            Proficiencies.Where(p => p.Type == ProficiencyType.Weapon).ToList();

        [NotMapped]
        public List<CharacterProficiency> ArmorProficiencies =>
            Proficiencies.Where(p => p.Type == ProficiencyType.Armor).ToList();

        [NotMapped]
        public List<CharacterProficiency> ToolProficiencies =>
            Proficiencies.Where(p => p.Type == ProficiencyType.Tool).ToList();

        public List<CharacterFeat> Feats { get; set; }

        public List<CharacterSpell> Spells { get; set; }

        public List<CharacterWeapon> Weapons { get; set; }

        public List<CharacterEquipment> Equipment { get; set; } 

        public CharacterCurrency Currency { get; set; }
    }
}
