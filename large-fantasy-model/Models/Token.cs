using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;


        public int X { get; set; } = 2500;
        public int Y { get; set; } = 2500;

        [Required]
        [MaxLength(7)]
        public string Color { get; set; } = "#0d6efd";
    }
}