using GameHubAPI.Data;
using GameHubAPI.DTOs.Games;
using GameHubAPI.Enums;
using GameHubAPI.Exceptions;
using GameHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHubAPI.Sevices
{
    public class GameLibraryNoteService
    {
        private readonly AppDbContext _context;

        public GameLibraryNoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameLibraryNoteDto> GetNoteByIdAsync(int entryId, int noteId, int currentUserId)
        {
            var entry = await _context.GameLibraryEntries.FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

            var note = await _context.gameLibraryNotes.FirstOrDefaultAsync(n => n.Id == noteId && n.GameLibraryEntryId == entryId);

            if (note is null)
                throw new NoteNotFoundException("Note was not found");

            return ToGameLibraryNoteDto(note);
        }

        public async Task<PagedResult<GameLibraryNoteDto>> GetNotesAsync(int entryId, int currentUserId, string? search, bool descending, int page, int pageSize)
        {
            var entry = await _context.GameLibraryEntries.FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

            IQueryable<GameLibraryNote> query = _context.gameLibraryNotes.Where(n => n.GameLibraryEntryId == entryId);

            if (page < 1)
                throw new InvalidPaginationException("Page must be greater than 0");

            if (pageSize < 1 || pageSize > 10)
                throw new InvalidPaginationException("Page size must be between 1 and 10");

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(n => EF.Functions.ILike(n.Title, $"%{search}%"));

            query = descending
                ? query.OrderByDescending(n => n.CreatedAt).ThenByDescending(n => n.Id)
                : query.OrderBy(n => n.CreatedAt).ThenBy(n => n.Id);

            var totalCount = await query.CountAsync();

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var notes = await query.Select(n => new GameLibraryNoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                CreatedAt = n.CreatedAt,
            }).ToListAsync();

            return new PagedResult<GameLibraryNoteDto>
            {
                Items = notes,
                TotalCount = totalCount
            };
        }

        public async Task<GameLibraryNoteDto> CreateNoteAsync(int entryId, int currentUserId, CreateGameLibraryNoteDto dto)
        {
            var entry = await _context.GameLibraryEntries.FirstOrDefaultAsync(e => e.Id == entryId && e.UserId == currentUserId);

            if (entry is null)
                throw new GameNotFoundException("Game was not found");

            var note = new GameLibraryNote
            {
                GameLibraryEntryId = entry.Id,
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.gameLibraryNotes.Add(note);

            await _context.SaveChangesAsync();

            return ToGameLibraryNoteDto(note);
        }

        private static GameLibraryNoteDto ToGameLibraryNoteDto(GameLibraryNote note)
        {
            return new GameLibraryNoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                CreatedAt= note.CreatedAt
            };
        }
    }
}
