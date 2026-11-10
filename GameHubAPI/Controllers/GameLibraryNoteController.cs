using GameHubAPI.DTOs.Games;
using GameHubAPI.Models;
using GameHubAPI.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameHubAPI.Controllers
{
    [ApiController]
    [Route("api/library/{entryId:int}/notes")]
    [Authorize]
    public class GameLibraryNoteController : BaseController
    {
        private readonly GameLibraryNoteService _gameLibraryNoteService;

        public GameLibraryNoteController(GameLibraryNoteService gameLibraryNoteService)
        {
            _gameLibraryNoteService = gameLibraryNoteService;
        }

        [HttpGet("{noteId:int}")]
        public async Task<ActionResult<GameLibraryNoteDto>> GetNoteById(int entryId, int noteId)
        {
            var note = await _gameLibraryNoteService.GetNoteByIdAsync(entryId, noteId, CurrentUserId);

            return Ok(note);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GameLibraryNoteDto>>> GetNotes(int entryId, string? search, bool descending, int page = 1, int pageSize = 10)
        {
            var notes = await _gameLibraryNoteService.GetNotesAsync(entryId, CurrentUserId, search, descending, page, pageSize);

            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult<GameLibraryNoteDto>> CreateNote(int entryId, CreateGameLibraryNoteDto dto)
        {
            var note = await _gameLibraryNoteService.CreateNoteAsync(entryId, CurrentUserId, dto);

            return CreatedAtAction(
                nameof(GetNoteById),
                new 
                { 
                    entryId,
                    noteId = note.Id
                },
                note);
        }

        [HttpDelete("{noteId:int}")]
        public async Task<IActionResult> DeleteNote(int entryId, int noteId)
        {
            await _gameLibraryNoteService.DeleteNoteAsync(entryId, noteId, CurrentUserId);

            return NoContent();
        }

        [HttpPut("{noteId:int}")]
        public async Task<IActionResult> UpdateNote(int entryId, int noteId, UpdateGameLibraryNoteDto dto)
        {
            var note = await _gameLibraryNoteService.UpdateNoteAsync(entryId, noteId, CurrentUserId, dto);

            return Ok(note);
        }
    }
}
