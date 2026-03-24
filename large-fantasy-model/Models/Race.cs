using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string SubType { get; set; }

        public string Size { get; set; }

        public List<string> Traits { get; set; }

        public List<Action> Actions { get; set; }
    }
}
