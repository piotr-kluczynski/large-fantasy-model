using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Spell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HigherLevel { get; set; }
        public string Level { get; set; }
        public string CastingTime { get; set; }
        public string RangeArea { get; set; }
        public List<string> Components { get; set; }
        public string Material { get; set; }
        public bool Ritual { get; set; }
        public bool Concetration { get; set; }
        public string Duration { get; set; }
        public string School { get; set; }
        public string AttackSave { get; set; }
        public string DamageEffect { get; set; }
    }
}
