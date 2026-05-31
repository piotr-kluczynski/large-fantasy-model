namespace large_fantasy_model.ViewModels.Compendium
{
    public class EntityDetailsViewModel
    {
        public string Title { get; set; }
        public string Rulebook { get; set; }
        public string Category { get; set; }

        public List<EntityFieldValue> Fields { get; set; } = new();
    }

    public class EntityFieldValue
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
