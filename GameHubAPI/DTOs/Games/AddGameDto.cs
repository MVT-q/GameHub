using GameHubAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameHubAPI.DTOs.Games
{
    public class AddGameDto
    {
        [Required]
        public string Title { get; set; } = "";

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public bool HasDedicatedServers { get; set; }

        [Required]
        public GameGenre Genre { get; set; }
    }
}
