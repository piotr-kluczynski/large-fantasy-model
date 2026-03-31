using large_fantasy_model.Models.CharacterModels;
using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 25 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must meet all complexity requirements listed below the field.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "It has to be stated whether the user has admin permissions.")]
        public int AdminPermissions { get; set; }


        // Kolekcja przechowująca gry w których uczestniczy użytkownik - relacja wiele-do-wielu (wielu użytkowników może brać udział w wielu grach)
        public ICollection<Game> Games { get; set; } = new List<Game>();

        // Kolekcja przechowująca wiadomości wysłane przez użytkownika - relacja jeden-do-wielu (jeden użytkownik może wysłać wiele wiadomości)
        public virtual ICollection<Message>? SentMessages { get; set; }

        // Kolekcja przechowująca konwersacje, w których bierze udział użytkownik - relacja wiele-do-wielu (wielu użytkowników może brać udział w wielu konwersacjach)
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        // Kolekcje służące do stworzenia relacji wiele-do-wielu dla tabeli User z tabelą User - funkcjonalność znajomych
        public ICollection<User> Friends { get; set; } = new List<User>();
        public ICollection<User> FriendOf { get; set; } = new List<User>();
    
        // Kolekcja przechowuiąca zasoby należące do użytkownika - relacja jeden-do-wielu (jeden użytkownik może mieć wiele zasobów)
        public virtual ICollection<Resources>? Resources { get; set; }

        // IN PROGRESS
        // Odwołanie do gier posiadanych przez użytkownika - relacja jeden-do-wielu (jeden użytkownik może być właścicielem wielu gier)
         public virtual ICollection<Game>? OwnedGames { get; set; }

        // Kolekcja przechowująca listę postaci należących do gracza - relacja wiele-do-wielu (wielu użytkowników może posiadać wiele postaci)
        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}
