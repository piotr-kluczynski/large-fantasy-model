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

        [Required(ErrorMessage = "Campaign description is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters long.")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastSessionDate { get; set; }

        // Kolekcja przechowująca graczy biorących udział w tej grze - relacja wiele-do-wielu (wielu użytkowników może brać udział w wielu grach)
        public ICollection<User> Users { get; set; } = new List<User>();

        // Kolekcja przechowująca postacie biorące udział w tej grze - relacja wiele-do-wielu
        public ICollection<large_fantasy_model.Models.CharacterModels.Character> Characters { get; set; } = new List<large_fantasy_model.Models.CharacterModels.Character>();

        // Odwołanie do konwersacji powiązanej z tą grą - relacja jeden-do-jednego, dziecko (jedna gra ma tylko jedną powiązaną konwersację)
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    
        // Odwołanie do właściciela gry - relacja jeden-do-wielu (jeden użytkownik może być właścicielem wielu gier)
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsPublic { get; set; } = false;

        [StringLength(10)]
        public string? JoinCode { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }
        public bool IsActive { get; set; } = true; 
        public int MaxPlayers { get; set; } = 10;
        public string? Lore { get; set; }
        public string? MapImageUrl { get; set; }
    }
}
