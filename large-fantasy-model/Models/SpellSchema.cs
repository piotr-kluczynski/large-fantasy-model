namespace large_fantasy_model.Models
{
    public static class SpellSchema
    {
        public static List<EntityFieldDefinition> Fields => new()
    {
        new() { Key = "Name", Label = "Name", Type = "text" },
        new() { Key = "Description", Label = "Description", Type = "textarea" },
        new() { Key = "Level", Label = "Level", Type = "number" },
        new() { Key = "School", Label = "School", Type = "text" },
        new() { Key = "CastingTime", Label = "Casting Time", Type = "text" },
        new() { Key = "RangeArea", Label = "Range", Type = "text" },
        new() { Key = "Duration", Label = "Duration", Type = "text" },
        new() { Key = "Ritual", Label = "Ritual", Type = "checkbox" },
        new() { Key = "Concentration", Label = "Concentration", Type = "checkbox" }
    };
    }
}
