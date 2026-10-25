using GameHubAPI.Enums;

namespace GameHubAPI.Models
{
    public class GameLibraryEntry
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public GameProgressStatus Status { get; set; }

        public float? Rating { get; set; }

        public ICollection<GameLibraryNote> Notes { get; set; } = [];

        public DateTime AddedAt { get; set; }
    }
}
