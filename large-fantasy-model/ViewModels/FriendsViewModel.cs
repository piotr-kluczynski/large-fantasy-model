namespace large_fantasy_model.ViewModels
{
    public class FriendsViewModel
    {
        public List<UserViewModel> MutualFriends { get; set; } = new List<UserViewModel>();
        public List<UserViewModel> ReceivedRequests { get; set; } = new List<UserViewModel>();
        public List<UserViewModel> SentRequests { get; set; } = new List<UserViewModel>();

        public List<UserViewModel> SearchResults { get; set; } = new List<UserViewModel>();
        public string SearchQuery { get; set; }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}