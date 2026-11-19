using GameHubAPI.DTOs.Games.Catalog;
using GameHubAPI.Enums;
using GameHubAPI.Models;
using GameHubAPI.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameHubAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GameListItemDto>>> GetGameList(string? search, GameGenre? genre, bool? hasDedicatedServer, GameSortBy? sortBy, bool descending, int page = 1, int pageSize = 50)
        {
            var games = await _gameService.GetGameListAsync(search, genre, hasDedicatedServer, sortBy, descending, page, pageSize);

            return Ok(games);
        }

        [HttpGet("{gameId:int}")]
        public async Task<ActionResult<GameDetailsDto>> GetGameById(int gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<GameDetailsDto>> AddGame(AddGameDto dto)
        {
            var game = await _gameService.AddGameAsync(dto);

            return CreatedAtAction(
                nameof(GetGameById),
                new { gameId = game.Id },
                game);
        }

        [HttpPut("{gameId:int}")]
        public async Task<ActionResult<GameDetailsDto>> UpdateGameInfo(int gameId, UpdateGameInfoDto dto)
        {
            var game = await _gameService.UpdateGameInfoAsync(gameId, dto);

            return Ok(game);
        }

        [HttpDelete("{gameId:int}")]
        public async Task<IActionResult> DeleteGame(int gameId)
        {
            await _gameService.DeleteGameAsync(gameId);

            return NoContent();
        }
    }
}
