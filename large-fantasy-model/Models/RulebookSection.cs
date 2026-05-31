using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models
{
    public class RulebookCategory
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string FilesPathName { get; set; }
        public List<RulebookItemViewModel> Items { get; set; } = new();
    }
}
