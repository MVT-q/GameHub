using GameHubAPI.DTOs.Games;
using GameHubAPI.Enums;
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

        [HttpGet]
        public async Task<ActionResult<GameLibraryEntryDetailsDto>> GetLibraryEntries(string? search, GameGenre? genre, GameProgressStatus? status, bool? sortByRating, int page = 1, int pageSize = 50)
        {
            var entry = await _gameLibraryService.GetLibraryEntriesAsync(CurrentUserId, search, genre, status, sortByRating, page, pageSize);

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

        [HttpPatch("{entryId:int}/status")]
        public async Task<ActionResult<GameLibraryEntryDetailsDto>> UpdateGameStatus(int entryId, UpdateGameStatusDto dto)
        {
            var entry = await _gameLibraryService.UpdateGameStatusAsync(entryId, CurrentUserId, dto);

            return Ok(entry);
        }

        [HttpPatch("{entryId:int}/rating")]
        public async Task<ActionResult<GameLibraryEntryDetailsDto>> UpdateGameRating(int entryId, UpdateGameRatingDto dto)
        {
            var entry = await _gameLibraryService.UpdateGameRatingAsync(entryId, CurrentUserId, dto);

            return Ok(entry);
        }
    }
}
