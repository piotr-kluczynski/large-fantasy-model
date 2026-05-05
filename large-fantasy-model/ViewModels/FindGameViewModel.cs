namespace large_fantasy_model.ViewModels
{
   
    public class FindGameViewModel
    {
        public string SearchQuery { get; set; }
        public List<GameListItemViewModel> Games { get; set; } = new();
    }

    
    public class GameListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool HasPassword { get; set; }
        public string DungeonMasterName { get; set; }
        public int PlayerCount { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAlreadyMember { get; set; }
    }
}