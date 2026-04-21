namespace large_fantasy_model.Models
{
    public class RulebookCategory
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public CategoryType Type { get; set; }
    }

    public enum CategoryType
    {
        Spells
    }
}
