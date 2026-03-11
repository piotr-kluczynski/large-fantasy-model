using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Conversation have to have a title.")]
        public string Title { get; set; }

        // Kolekcja przechowująca członków konwersacji - relacja wiele-do-wielu (wielu użytkowników może uczestniczyć w wielu konwersacjach)
        public ICollection<User> Users { get; set; } = new List<User>();

        // Kolekcja przechowująca wiadomości należące do tej konwersacji - relacja jeden-do-wielu (wiele wiadomości może należeć do jednej konwersacji)
        public ICollection<Message>? Messages { get; set; }

        // Odwołanie do gry, z którą powiązana jest ta konwersacja - relacja jeden-do-jednego, rodzic (jedna gra może mieć tylko jedną powiązaną konwersacją)
        public Game? Game { get; set; }
    }
}
