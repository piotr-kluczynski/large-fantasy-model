namespace large_fantasy_model.ViewModels.Compendium
{
    public class RulebookDetailsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconEmoji { get; set; }
        public string PdfFileName { get; set; }
        public string FilesPathName { get; set; }

        public List<RulebookCategoryViewModel> Categories { get; set; } = new();
    }
}
