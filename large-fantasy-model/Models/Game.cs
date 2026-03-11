using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

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

        // Kolekcja przechowująca graczy biorących udział w tej grze - relacja wiele-do-wielu (wielu użytkowników może brać udział w wielu grach)
        public ICollection<User> Users { get; set; } = new List<User>();

        // Odwołanie do konwersacji powiązanej z tą grą - relacja jeden-do-jednego, dziecko (jedna gra ma tylko jedną powiązaną konwersację)
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    
        // Odwołanie do właściciela gry - relacja jeden-do-wielu (jeden użytkownik może być właścicielem wielu gier)
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
