using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.Models.CharacterModels.References;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models.CharacterModels.Additional.Creature
{
    public class Creature
    {
        public string Name { get; set; }

        public string Alignment { get; set; }

        public Speed Speed { get; set; }

        public HitPoints HitPoints { get; set; }

        public string Inspiration { get; set; }

        public Skills Skills { get; set; }

        public List<Language> Languages { get; set; }

        public AbilityScores AbilityScores { get; set; }

        public SavingThrows SavingThrows { get; set; }

        public Senses Senses { get; set; }

        public ArmorClass ArmorClass { get; set; }

        public bool Shield { get; set; }

        public Conditions Conditions { get; set; }

        public Conditions ConditionImmunities { get; set; }

        /* Ze względu na tworzenie relacji przesunąłem tą cechę poziom wyżej - do klas które "dziedziczą" Creature
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
        */
    }
}
