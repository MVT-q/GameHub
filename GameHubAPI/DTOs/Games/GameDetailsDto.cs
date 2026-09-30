using GameHubAPI.Enums;

namespace GameHubAPI.DTOs.Games
{
    public class GameDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; } = "";

        public GameGenre Genre { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
