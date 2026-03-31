namespace large_fantasy_model.Models.Character.Json
{
    public class Weapon : Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // Change to enum
        
        public Damage Damage {  get; set; }

        public Dice VersatileDamage { get; set; }

        public Range Range { get; set; }

        public Range ThrowRange { get; set; }

        public WeaponProperties Properties { get; set; }
    }
}
