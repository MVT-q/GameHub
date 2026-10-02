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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GameDetailsDto>> AddGame(AddGameDto dto)
        {
            var game = await _gameService.AddGameAsync(dto);

            return CreatedAtAction(
                nameof(GetGameById),
                new { gameId = game.Id },
                game);
        }

        [Authorize]
        [HttpPut("{gameId:int}")]
        public async Task<ActionResult<GameDetailsDto>> UpdateGameInfo(int gameId, UpdateGameInfoDto dto)
        {
            var game = await _gameService.UpdateGameInfoAsync(gameId, dto);

            if (game == null)
                return NotFound();

            return Ok(game);
        }
    }
}
