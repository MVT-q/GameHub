using GameHubAPI.DTOs.Games;
using GameHubAPI.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameHubAPI.Controllers
{
    [ApiController]
    [Route("api/library")]
    [Authorize]
    public class GameLibraryController : BaseController
    {
        private readonly GameLibraryService _gameLibraryService;

        public GameLibraryController(GameLibraryService gameLibraryService)
        {
            _gameLibraryService = gameLibraryService;
        }

        [HttpPost]
        public async Task<ActionResult<GameLibraryEntryDto>> AddGameToLibrary(AddGameToLibraryDto dto)
        {
            var game = await _gameLibraryService.AddGameToLibraryAsync(CurrentUserId, dto);

            return StatusCode(StatusCodes.Status201Created, game);
        }
    }
}
