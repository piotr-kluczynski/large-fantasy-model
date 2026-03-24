using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // Relacja wiele-do-wielu z Class
    }
}
