using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace large_fantasy_model.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        
        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

        
        public int? SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        
        [Required]
        public string Type { get; set; }

        [Required]
        public string Message { get; set; }

        
        public int? RelatedEntityId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}