namespace large_fantasy_model.ViewModels
{
    public class GameLobbyViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JoinCode { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDungeonMaster { get; set; }

        public UserViewModel DungeonMaster { get; set; }
        public List<UserViewModel> Players { get; set; } = new();

        
        public List<UserViewModel> AvailableFriends { get; set; } = new();
    }
}