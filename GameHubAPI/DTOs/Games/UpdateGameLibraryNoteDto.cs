using System.ComponentModel.DataAnnotations;

namespace GameHubAPI.DTOs.Games
{
    public class UpdateGameLibraryNoteDto
    {
        [Required]
        public string Title { get; set; } = "";

        public string? Description { get; set; }
    }
}
