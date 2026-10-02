using GameHubAPI.Data;
using GameHubAPI.DTOs.Games;
using GameHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHubAPI.Sevices
{
    public class GameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GameListItemDto>> GetGameListAsync()
        {
            var games = await _context.Games.ToListAsync();

            return games.Select(ToGameListDto).ToList();
        }

        public async Task<GameDetailsDto?> GetGameByIdAsync(int gameId)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                return null;

            return ToGameDetailsDto(game);
        }

        public async Task<GameDetailsDto> AddGameAsync(AddGameDto dto)
        {
            var game = new Game
            {
                Title = dto.Title,
                ReleaseDate = dto.ReleaseDate,
                Description = dto.Description,
                HasDedicatedServers = dto.HasDedicatedServers,
                Genre = dto.Genre,
                CreatedAt = DateTime.UtcNow
            };

            _context.Games.Add(game);

            await _context.SaveChangesAsync();

            return ToGameDetailsDto(game);
        }

        public async Task<GameDetailsDto?> UpdateGameInfoAsync(int gameId, UpdateGameInfoDto dto)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                return null;

            game.Title = dto.Title;
            game.ReleaseDate = dto.ReleaseDate;
            game.Description = dto.Description;
            game.HasDedicatedServers = dto.HasDedicatedServers;
            game.Genre = dto.Genre;
            game.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ToGameDetailsDto(game);
        }

        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                return false;

            _context.Games.Remove(game);

            await _context.SaveChangesAsync();

            return true;
        }

        private static GameListItemDto ToGameListDto(Game game)
        {
            return new GameListItemDto
            {
                Id = game.Id,
                Title = game.Title
            };
        }

        private static GameDetailsDto ToGameDetailsDto(Game game)
        {
            return new GameDetailsDto
            {
                Id = game.Id,
                Title = game.Title,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description,
                Genre = game.Genre,
                UpdatedAt = game.UpdatedAt
            };
        }
    }
}
