namespace large_fantasy_model.Models.Character.Additional.Creature
{
    public class Speed
    {
        public int Walk { get; set; } 
        public SpeedValue Burrow { get; set; }
        public SpeedValue Climb { get; set; }
        public SpeedValue Fly { get; set; }
        public SpeedValue Hover { get; set; }
        public SpeedValue Swim { get; set; }
    }
}
