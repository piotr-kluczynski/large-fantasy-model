using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class GameChatMessage
    {
        [Key]
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        [Required]
        public string SenderName { get; set; } = null!;

        [Required]
        public string Text { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}