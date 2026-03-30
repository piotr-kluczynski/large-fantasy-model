using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character.Json
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int HitDie { get; set; } // Change to enum

        public string Spellcasting { get; set; } = ""; // Change to enum

        public List<Feature> Features { get; set; }
    }
}
