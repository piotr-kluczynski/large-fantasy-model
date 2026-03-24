using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character.Json
{
    public class Class
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public string Subtype { get; set; }

        public int HitDie { get; set; } // Change to enum

        public string Spellcasting { get; set; } = "";

        public List<Feature> Features { get; set; }
    }
}
