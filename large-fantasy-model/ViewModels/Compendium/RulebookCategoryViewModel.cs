namespace large_fantasy_model.ViewModels.Compendium
{
    public class RulebookCategoryViewModel
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string FilesPathName { get; set; }

        public List<RulebookItemViewModel> Items { get; set; } = new();
    }
}
