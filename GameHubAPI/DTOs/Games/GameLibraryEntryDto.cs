namespace GameHubAPI.DTOs.Games
{
    public class GameLibraryEntryDto
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Title { get; set; } = "";

        public float? Rating { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
