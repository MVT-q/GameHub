namespace GameHubAPI.Models
{
    public class GameLibraryNote
    {
        public int Id { get; set; }

        public int GameLibraryEntryId { get; set; }

        public GameLibraryEntry GameLibraryEntry { get; set; } = null!;

        public string Title { get; set; } = "";

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
