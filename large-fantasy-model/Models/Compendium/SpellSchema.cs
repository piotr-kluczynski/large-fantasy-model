using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    /// <summary>
    /// "Schemat" opisujący pola oraz ich właściwości elementów należących do kategorii "Spells" systemu RPG "DnD Basic Rules 2018".
    /// Służy on do łatwiejszego poprawnego reprezentowania pól w widokach.
    /// </summary>
    public static class SpellSchema
    {
        /// <summary>
        /// Lista obiektów EntityFieldDefinition, które opisują pola elementu kategorii "Spell".
        /// </summary>
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
