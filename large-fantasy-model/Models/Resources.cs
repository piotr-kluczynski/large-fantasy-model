using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Resources
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int IsPublic { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Name of the resource has to be provided.")]
        public string Name { get; set; }


        [StringLength(100, ErrorMessage = "The description of the resource can't be longer than 100 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string FilePath { get; set; }

        // Odwołanie do właściciela zasobu - relacja jeden-do-wielu (jeden użytkownik może posiadać wiele zasobów)
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
