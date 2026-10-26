namespace GameHubAPI.DTOs.Games
{
    public class GameLibraryNoteDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
