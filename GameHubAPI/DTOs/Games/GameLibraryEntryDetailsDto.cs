using GameHubAPI.Enums;

namespace GameHubAPI.DTOs.Games
{
    public class GameLibraryEntryDetailsDto
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Title { get; set; } = "";

        public GameGenre Genre { get; set; }

        public GameProgressStatus Status { get; set; }

        public float? Rating { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
