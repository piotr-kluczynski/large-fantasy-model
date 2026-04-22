namespace large_fantasy_model.Models
{
    public class RulebookCategory
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string FilesPathName { get; set; }
        public List<IRulebookEntity> Items { get; set; } = new();
    }
}
