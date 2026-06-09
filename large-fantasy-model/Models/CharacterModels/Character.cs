using large_fantasy_model.Models.CharacterModels.Additional;
using large_fantasy_model.Models.CharacterModels.References;
using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.CharacterModels
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Xp { get; set; } = 0;

        public int Level { get; set; }

        public CharacterRace Race { get; set; }
        public CharacterClass Class { get; set; }
        public CharacterBackground Background { get; set; }
        public CharacterDetails Details { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();


        public List<CharacterFeature> Features { get; set; }

        public List<CharacterSpell> Spells { get; set; }

        public List<CharacterWeapon> Weapons { get; set; }

        public List<CharacterEquipment> Equipment { get; set; } 

        // Creature properties
        public string? Alignment { get; set; }
        public int Speed { get; set; }
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int TempHitPoints { get; set; }
        public int Inspiration { get; set; }
        public int ArmorClass { get; set; }
        public string? Languages { get; set; }

        public CharacterAbilityScores AbilityScores { get; set; }
        public CharacterSavingThrows SavingThrows { get; set; }
        public CharacterSkills Skills { get; set; }
    }
}
