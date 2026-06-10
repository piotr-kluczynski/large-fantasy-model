namespace large_fantasy_model.ViewModels.Compendium
{
    public class EntityFieldDefinition
    {
        public string Key { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }

        public bool ShowInDetails { get; set; } = true;
        public string? SourceCategory { get; set; }
    }
}
