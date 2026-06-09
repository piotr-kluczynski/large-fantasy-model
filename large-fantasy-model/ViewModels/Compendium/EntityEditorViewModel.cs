namespace large_fantasy_model.ViewModels.Compendium
{
    public class EntityEditorViewModel
    {
        public string Rulebook { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public bool IsEdit { get; set; }

        public List<EntityFieldDefinition> Fields { get; set; }

        public Dictionary<string, string?> Values { get; set; } = new();

        public Dictionary<string, List<EntitySelectOption>> AvailableOptions { get; set; } = new();

        public Dictionary<string, List<string>> ListValues { get; set; } = new();
    }

    public class EntitySelectOption
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
