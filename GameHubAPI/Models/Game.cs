using GameHubAPI.Enums;

namespace GameHubAPI.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; } = "";

        public bool HasDedicatedServers { get; set; }

        public GameGenre Genre { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
