namespace large_fantasy_model.ViewModels
{
    public class ActiveGameViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDungeonMaster { get; set; }

        public UserViewModel DungeonMaster { get; set; } = null!;
        public List<UserViewModel> Players { get; set; } = new();
        public List<TokenViewModel> Tokens { get; set; } = new();
        public List<ChatMessageViewModel> ChatMessages { get; set; } = new();
        public string? MapImageUrl { get; set; }
    }

    public class TokenViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; } = null!;
    }

    public class ChatMessageViewModel
    {
        public string SenderName { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string Time { get; set; } = null!;
    }
}