using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 25 characters.")]
        public string Username { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "It has to be stated whether the user has admin permissions.")]
        public int AdminPermissions { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();

        public virtual ICollection<Message>? Messages { get; set; }

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        public ICollection<User> Friends { get; set; } = new List<User>();
        public ICollection<User> FriendOf { get; set; } = new List<User>();
    }
}
