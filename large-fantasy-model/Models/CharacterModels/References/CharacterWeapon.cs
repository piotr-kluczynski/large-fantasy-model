namespace large_fantasy_model.Models.CharacterModels.References
{
    public class CharacterWeapon
    {
        public int Id { get; set; }
        public string WeaponName { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
