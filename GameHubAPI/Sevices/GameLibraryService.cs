using GameHubAPI.Data;
using GameHubAPI.DTOs.Games;
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

            var alreadyAdded = await _context.GameLibraryEntries.AnyAsync(gle => gle.GameId == dto.GameId && gle.UserId == currentUserId);

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
                .Include(e => e.Notes)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

            return ToGameLibraryEntryDetailsDto(entry);
        }

        public async Task<bool> DeleteGameEntryAsync(int entryId, int currentUserId)
        {
            var entry = await _context.GameLibraryEntries.FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if(entry is null)
                return false;

            _context.GameLibraryEntries.Remove(entry);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<GameLibraryEntryDetailsDto> UpdateGameStatusAsync(int entryId, int currentUserId, UpdateGameStatusDto dto)
        {
            var entry = await _context.GameLibraryEntries
                .Include(e => e.Game)
                .Include(e => e.Notes)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

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
                .Include(e => e.Notes)
                .FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

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
                Notes = entry.Notes.Select(n => new GameLibraryNoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description
                }).ToList()
            };
        }
    }
}
