using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string  Content { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SendingTime { get; set; }

        public virtual Conversation Conversation { get; set; }

        public virtual User Sender { get; set; }
    }
}
