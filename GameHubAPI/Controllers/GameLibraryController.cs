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
            var entry = await _gameLibraryService.AddGameToLibraryAsync(CurrentUserId, dto);

            return CreatedAtAction(
                nameof(GetGameEntryById),
                new { entryId = entry.Id },
                entry);
        }

        [HttpGet("{entryId:int}")]
        public async Task<ActionResult<GameLibraryEntryDetailsDto>> GetGameEntryById(int entryId)
        {
            var entry = await _gameLibraryService.GetGameEntryByIdAsync(entryId, CurrentUserId);

            return Ok(entry);
        }

        [HttpDelete("{entryId:int}")]
        public async Task<IActionResult> DeleteGameEntry(int entryId)
        {
            var result = await _gameLibraryService.DeleteGameEntryAsync(entryId, CurrentUserId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
