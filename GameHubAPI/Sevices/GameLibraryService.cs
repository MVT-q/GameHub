using GameHubAPI.Data;
using GameHubAPI.DTOs.Games.Library;
using GameHubAPI.Enums;
using GameHubAPI.Exceptions;
using GameHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHubAPI.Sevices
{
    public class GameLibraryService
    {
        private readonly AppDbContext _context;

        public GameLibraryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameLibraryEntryDto> AddGameToLibraryAsync(int currentUserId, AddGameToLibraryDto dto)
        {
            var catalogGame = await _context.Games.FirstOrDefaultAsync(g => g.Id == dto.GameId);

            if (catalogGame is null)
                throw new GameNotFoundException("Game was not found");

            var alreadyAdded = await _context.GameLibraryEntries.AnyAsync(e => e.GameId == dto.GameId && e.UserId == currentUserId);

            if (alreadyAdded)
                throw new GameAlreadyInLibraryException("Game is already in your library");

            var game = new GameLibraryEntry
            {
                GameId = dto.GameId,
                UserId = currentUserId,
                Status = GameProgressStatus.WantToPlay,
                AddedAt = DateTime.UtcNow
            };

            _context.GameLibraryEntries.Add(game);

            await _context.SaveChangesAsync();

            return new GameLibraryEntryDto
            {
                Id = game.Id,
                GameId = dto.GameId,
                Title = catalogGame.Title
            };
        }

        public async Task<GameLibraryEntryDetailsDto> GetGameEntryByIdAsync(int entryId, int currentUserId)
        {
            var entry = await _context.GameLibraryEntries
                .Include(e => e.Game)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameLibraryEntryNotFoundException("Game was not found");

            return ToGameLibraryEntryDetailsDto(entry);
        }

        public async Task<PagedResult<GameLibraryEntryDto>> GetLibraryEntriesAsync(int currentUserId, string? search, GameGenre? genre, GameProgressStatus? status, bool? sortByRating, int page, int pageSize)
        {
            IQueryable<GameLibraryEntry> query = _context.GameLibraryEntries.Where(e => e.UserId == currentUserId);

            if (page < 1)
                throw new InvalidPaginationException("Page must be greater than 0");

            if (pageSize < 1 || pageSize > 50)
                throw new InvalidPaginationException("Page size must be between 1 and 50");

            if (genre.HasValue)
                query = query.Where(e => e.Game.Genre == genre);

            if (status.HasValue)
                query = query.Where(e => e.Status == status);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => EF.Functions.ILike(e.Game.Title, $"%{search}%"));

            if (genre.HasValue && !Enum.IsDefined(genre.Value))
                throw new InvalidGameGenreException("Invalid game genre");

            if (status.HasValue && !Enum.IsDefined(status.Value))
                throw new InvalidGameStatusException("Invalid game status");

            if (sortByRating.HasValue)
            {
                query = sortByRating.Value
                    ? query.OrderBy(e => e.Rating == null)
                           .ThenByDescending(e => e.Rating)
                           .ThenByDescending(e => e.Id)
                    : query.OrderBy(e => e.Rating == null)
                           .ThenBy(e => e.Rating)
                           .ThenByDescending(e => e.Id);
            }
            else
            {
                query = query.OrderByDescending(e => e.AddedAt)
                             .ThenByDescending(e => e.Id);
            }

            var totalCount = await query.CountAsync();

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var entries = await query.Select(e => new GameLibraryEntryDto
            {
                Id = e.Id,
                GameId = e.GameId,
                Title = e.Game.Title,
                Rating = e.Rating,
                AddedAt = e.AddedAt
            }).ToListAsync();

            return new PagedResult<GameLibraryEntryDto>
            {
                Items = entries,
                TotalCount = totalCount
            };
        }

        public async Task DeleteGameEntryAsync(int entryId, int currentUserId)
        {
            var entry = await _context.GameLibraryEntries.FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameLibraryEntryNotFoundException("Game was not found");

            _context.GameLibraryEntries.Remove(entry);

            await _context.SaveChangesAsync();
        }

        public async Task<GameLibraryEntryDetailsDto> UpdateGameStatusAsync(int entryId, int currentUserId, UpdateGameStatusDto dto)
        {
            var entry = await _context.GameLibraryEntries
                .Include(e => e.Game)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameLibraryEntryNotFoundException("Game was not found");

            if (!Enum.IsDefined(dto.Status))
                throw new InvalidGameStatusException("Invalid game status");

            entry.Status = dto.Status;

            await _context.SaveChangesAsync();

            return ToGameLibraryEntryDetailsDto(entry);
        }

        public async Task<GameLibraryEntryDetailsDto> UpdateGameRatingAsync(int entryId, int currentUserId, UpdateGameRatingDto dto)
        {
            var entry = await _context.GameLibraryEntries
                .Include(e => e.Game)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameLibraryEntryNotFoundException("Game was not found");

            if (dto.Rating.HasValue)
            {
                var rating = dto.Rating.Value;

                if (rating < 0 || rating > 10)
                    throw new InvalidGameRatingException("Rating must be between 0 and 10");

                if (MathF.Abs(rating * 2 - MathF.Round(rating * 2)) > 0.0001f)
                    throw new InvalidGameRatingException("Rating must use increments of 0.5");
            }

            entry.Rating = dto.Rating;

            await _context.SaveChangesAsync();

            return ToGameLibraryEntryDetailsDto(entry);
        }

        public async Task<bool> IsGameInLibraryAsync(int gameId, int currentUserId)
        {
            return await _context.GameLibraryEntries.AnyAsync(e => e.GameId == gameId && e.UserId == currentUserId);
        }

        private static GameLibraryEntryDetailsDto ToGameLibraryEntryDetailsDto(GameLibraryEntry entry)
        {
            return new GameLibraryEntryDetailsDto
            {
                Id = entry.Id,
                GameId = entry.GameId,
                Title = entry.Game.Title,
                Genre = entry.Game.Genre,
                Status = entry.Status,
                Rating = entry.Rating,
                AddedAt = entry.AddedAt,
            };
        }
    }
}
