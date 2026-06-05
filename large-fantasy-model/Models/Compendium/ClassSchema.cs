using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    public static class ClassSchema
    {
        public static List<EntityFieldDefinition> Fields => new()
        {
            new() { Key = "Name", Label = "Name", Type = "text" },
            new() { Key = "Description", Label = "Description", Type = "textarea" },
            new() { Key = "HitDie", Label = "Hit Die", Type = "text" },
            new() { Key = "Spellcasting", Label = "Spellcasting", Type = "checkbox" },
            // new() { Key = "Features", Label = "Features", Type = "list" } to be implemented
        };
    }
}
