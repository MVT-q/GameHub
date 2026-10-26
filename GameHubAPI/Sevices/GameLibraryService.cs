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
                Title = game.Game.Title
            };
        }
    }
}
