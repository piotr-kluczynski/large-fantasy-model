namespace large_fantasy_model.Models.Character.References
{
    public class CharacterWeapon
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string WeaponId { get; set; }
        public bool Equipped { get; set; }
    }
}
