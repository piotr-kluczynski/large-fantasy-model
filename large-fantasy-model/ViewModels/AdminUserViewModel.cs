namespace large_fantasy_model.ViewModels
{
    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdminPermissions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}