using GameHubAPI.Data;
using GameHubAPI.DTOs.Games;
using GameHubAPI.Enums;
using GameHubAPI.Exceptions;
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

        public async Task<PagedResult<GameListItemDto>> GetGameListAsync(string? search, GameGenre? genre, bool? hasDedicatedServer, GameSortBy? sortBy, bool descending, int page, int pageSize)
        {
            IQueryable<Game> query = _context.Games;

            if (page < 1)
                throw new InvalidPaginationException("Page must be greater than 0");

            if (pageSize < 1 || pageSize > 50)
                throw new InvalidPaginationException("Page size must be between 1 and 50");

            if (genre != null)
                query = query.Where(g => g.Genre == genre);

            if (hasDedicatedServer.HasValue)
                query = query.Where(g => g.HasDedicatedServers == hasDedicatedServer.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => EF.Functions.ILike(g.Title, $"%{search}%"));

            switch (sortBy)
            {
                case GameSortBy.Title:
                    if (descending)
                        query = query.OrderByDescending(g => g.Title);
                    else
                        query = query.OrderBy(g => g.Title);
                    break;

                case GameSortBy.ReleaseDate:
                    if (descending)
                        query = query.OrderByDescending(g => g.ReleaseDate);
                    else
                        query = query.OrderBy(g => g.ReleaseDate);
                    break;         
            }

            var totalCount = await query.CountAsync();

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var games = await query.Select(g => new GameListItemDto
            {
                Id = g.Id,
                Title = g.Title
            }).ToListAsync();

            return new PagedResult<GameListItemDto> 
            {
                Items = games,
                TotalCount = totalCount
            };
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
                HasDedicatedServers = game.HasDedicatedServers,
                Genre = game.Genre,
                UpdatedAt = game.UpdatedAt
            };
        }
    }
}
