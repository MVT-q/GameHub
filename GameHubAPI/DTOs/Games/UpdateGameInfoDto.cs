using GameHubAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameHubAPI.DTOs.Games
{
    public class UpdateGameInfoDto
    {
        [Required]
        public string Title { get; set; } = "";

        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public bool HasDedicatedServers { get; set; }

        public GameGenre Genre { get; set; }
    }
}
