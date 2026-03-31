using large_fantasy_model.Models.Character.Json;
using large_fantasy_model.Models.Character.References;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models.Character.Additional.Creature
{
    public class Creature
    {
        public string Name { get; set; }

        public string Alignment { get; set; }

        public Speed Speed { get; set; }

        public HitPoints HitPoints { get; set; }

        public string Inspiration { get; set; }

        public Skills Skills { get; set; }

        public List<string> Languages { get; set; }

        public AbilityScores AbilityScores { get; set; }

        public SavingThrows SavingThrows { get; set; }

        public Senses Senses { get; set; }

        public ArmorClass ArmorClass { get; set; }

        public bool Shield { get; set; }

        public Conditions Conditions { get; set; }

        public List<string> ConditionImmunities { get; set; }

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

    }
}
