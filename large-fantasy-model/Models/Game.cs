using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Game
    {
        [Key]
        public int GameKey { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Name of the game has to be provided.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastSessionDate { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    }
}
