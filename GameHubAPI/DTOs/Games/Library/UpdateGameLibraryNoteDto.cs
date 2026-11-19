using System.ComponentModel.DataAnnotations;

namespace GameHubAPI.DTOs.Games.Library
{
    public class UpdateGameLibraryNoteDto
    {
        [Required]
        public string Title { get; set; } = "";

        public string? Description { get; set; }
    }
}
