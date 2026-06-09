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

        public bool IsActive { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public UserViewModel DungeonMaster { get; set; }
        public List<UserViewModel> Players { get; set; } = new();

        
        public List<UserViewModel> AvailableFriends { get; set; } = new();
        public List<int> InvitedFriendIds { get; set; } = new();
        public string? Lore { get; set; }

        public Dictionary<int, string> PlayerCharacters { get; set; } = new();
        public List<large_fantasy_model.Models.CharacterModels.Character> AvailableCharacters { get; set; } = new();
        public int? CurrentUserSelectedCharacterId { get; set; }
    }
}