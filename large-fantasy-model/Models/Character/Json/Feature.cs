using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character.Json  
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
