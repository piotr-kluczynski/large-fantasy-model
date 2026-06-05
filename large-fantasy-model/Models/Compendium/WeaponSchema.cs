using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    public static class WeaponSchema
    {
        public static List<EntityFieldDefinition> Fields => new()
        {
            new() { Key = "Name", Label = "Name", Type = "text" },
            new() { Key = "Weight", Label = "Weight", Type = "number" },
            new() { Key = "Description", Label = "Description", Type = "textarea" },
            new() { Key = "Magic", Label = "Magic", Type = "checkbox" },
            new() { Key = "Damage", Label = "Damage", Type = "string" },
            new() { Key = "Range", Label = "Range", Type = "number" },
            new() { Key = "ThrowRange", Label = "Throw Range", Type = "number" },
            new() { Key = "Ammunition", Label = "Ammunition", Type = "text" },
            new() { Key = "Finesse", Label = "Finesse", Type = "checkbox" },
            new() { Key = "Heavy", Label = "Heavy", Type = "checkbox" },
            new() { Key = "Light", Label = "Light", Type = "checkbox" },
            new() { Key = "Loading", Label = "Loading", Type = "checkbox" },
            new() { Key = "Monk", Label = "Monk", Type = "checkbox" },
            new() { Key = "Reach", Label = "Reach", Type = "checkbox" },
            new() { Key = "Thrown", Label = "Thrown", Type = "checkbox" },
            new() { Key = "TwoHanded", Label = "Two Handed", Type = "checkbox" },
            new() { Key = "Versatile", Label = "Versatile", Type = "checkbox" },
        };
    }
}
