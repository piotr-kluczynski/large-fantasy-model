using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string  Content { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SendingTime { get; set; }

        // Odwołanie do obiektu konwersacji, do której należy ta wiadomość - relacja jeden-do-wielu (wiele wiadomości może należeć do jednej konwersacji)
        public virtual Conversation Conversation { get; set; }

        // Odwołanie do obiektu użytkownika, który wysłał wiadomość - relacja jeden-do-wielu (jeden użytkownik może wysłać wiele wiadomości)
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
    }
}
