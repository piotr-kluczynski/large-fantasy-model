namespace large_fantasy_model.ViewModels
{
    public class CreateSpellViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HigherLevel { get; set; }
        public string Level { get; set; }
        public string CastingTime { get; set; }
        public string RangeArea { get; set; }
        public List<string> Components { get; set; } = new();
        public string Material { get; set; }
        public bool Ritual { get; set; }
        public bool Concentration { get; set; }
        public string Duration { get; set; }
        public string School { get; set; }
        public string AttackSave { get; set; }
        public string DamageEffect { get; set; }
    }
}
