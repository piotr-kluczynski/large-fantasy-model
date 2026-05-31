using System.IO;

namespace large_fantasy_model.Helpers
{
    public static class AvatarHelper
    {
        public static string GetUserColor(string username)
        {
            if (string.IsNullOrEmpty(username)) return "0d6efd";
            var colors = new[] { "0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0" };
            int hash = 0;
            foreach (char c in username) { hash += c; }
            return colors[hash % colors.Length];
        }

        public static string GetUserAvatar(string username, string profilePicturePath, string webRootPath, int size = 120)
        {
            if (!string.IsNullOrEmpty(profilePicturePath))
            {
                var fullPath = Path.Combine(webRootPath, profilePicturePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    return profilePicturePath;
                }
            }
            return $"https://ui-avatars.com/api/?name={username}&size={size}&background={GetUserColor(username)}&color=fff&length=2";
        }
    }
}
