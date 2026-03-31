using large_fantasy_model.Models.CharacterModels.Additional;
using large_fantasy_model.Models.CharacterModels.Additional.Creature;
using large_fantasy_model.Models.CharacterModels.References;
using large_fantasy_model.Models.CharacterModels.Additional;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models.CharacterModels
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

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

        public List<CharacterDamageType> DamageTypes { get; set; }

        [NotMapped]
        public List<CharacterDamageType> DamageImmunities =>
           DamageTypes.Where(p => p.Category == DamageCategory.Immunity).ToList();

        [NotMapped]
        public List<CharacterDamageType> DamageResistances =>
           DamageTypes.Where(p => p.Category == DamageCategory.Resistance).ToList();

        [NotMapped]
        public List<CharacterDamageType> Vulnerabilities =>
           DamageTypes.Where(p => p.Category == DamageCategory.Vulnerability).ToList();

        public List<CharacterFeat> Feats { get; set; }

        public List<CharacterSpell> Spells { get; set; }

        public List<CharacterWeapon> Weapons { get; set; }

        public List<CharacterEquipment> Equipment { get; set; } 

        public CharacterCurrency Currency { get; set; }
    }
}
