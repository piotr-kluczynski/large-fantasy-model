using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    public static class RaceSchema
    {
        public static List<EntityFieldDefinition> Fields => new()
        {
            new() { Key = "Name", Label = "Name", Type = "text" },
            new() { Key = "Description", Label = "Description", Type = "textarea" },
            new() { Key = "Size", Label = "Size", Type = "text" },
            // new() { Key = "Traits", Label = "Traits", Type = "list" } to be implemented
        };
    }
}
