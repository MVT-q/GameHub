using GameHubAPI.Enums;

namespace GameHubAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string PasswordHash { get; private set; } = "";

        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<GameLibraryEntry> GameLibraryEntries { get; set; } = [];

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
