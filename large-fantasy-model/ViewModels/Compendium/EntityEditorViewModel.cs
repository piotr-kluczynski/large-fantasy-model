using large_fantasy_model.Models;

namespace large_fantasy_model.ViewModels.Compendium
{
    public class EntityEditorViewModel
    {
        public string Rulebook { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public List<EntityFieldDefinition> Fields { get; set; }
        public Dictionary<string, string> Values { get; set; } = new();
    }
}
