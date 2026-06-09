using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Conversation have to have a title.")]
        public string Title { get; set; }


        public ICollection<User> Users { get; set; } = new List<User>();


        public ICollection<Message>? Messages { get; set; }


        public Game? Game { get; set; }
    }
}
