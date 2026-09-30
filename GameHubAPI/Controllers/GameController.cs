using GameHubAPI.DTOs.Games;
using GameHubAPI.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameHubAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameListItemDto>>> GetGameList()
        {
            var games = await _gameService.GetGameListAsync();

            return Ok(games);
        }

        [Authorize]
        [HttpGet("{gameId:int}")]
        public async Task<ActionResult<GameDetailsDto>> GetGameById(int gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);

            if (game == null)
                return NotFound();

            return Ok(game);
        }
    }
}
