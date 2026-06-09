using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    public static class ItemSchema
    {
        public static List<EntityFieldDefinition> Fields => new()
        {
            new() { Key = "Name", Label = "Name", Type = "text" },
            new() { Key = "Weight", Label = "Weight", Type = "number" },
            new() { Key = "Description", Label = "Description", Type = "textarea" },
            new() { Key = "Magic", Label = "Magic", Type = "checkbox" }
        };
    }
}
