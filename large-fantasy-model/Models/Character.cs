using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Owner {  get; set; }
    }
}
