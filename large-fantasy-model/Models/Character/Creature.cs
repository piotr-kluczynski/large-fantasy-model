using large_fantasy_model.Models.Character.Json;

namespace large_fantasy_model.Models.Character
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

        public List<DamageType> DamageImmunities { get; set; }

        public List<DamageType> DamageResistances { get; set; }

        public List<DamageType> Vulnerabilities { get; set; }

        // Na razie pomijamy obraz postaci
    }
}
