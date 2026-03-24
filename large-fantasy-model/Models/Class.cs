using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Level { get; set; }

        public string Subtype { get; set; }

        public int HitDie { get; set; }

        public string Spellcasting { get; set; } = "";

        // Many-to-Many relation with "Features"
    }
}
