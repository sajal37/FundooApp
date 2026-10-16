using ModelLayer.Model;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteService;
        public NotesController(INoteBL noteService)
        {
            this.noteService = noteService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return false;
            }

            return int.TryParse(claim.Value, out userId);
        }

        [HttpPost("addnotes")]
        [Authorize]
        public async Task<IActionResult> AddNoteAsync(NoteCreationModel note)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }
            NotesIdModel noteId = await noteService.AddNoteAsync(note, currentUserId);
            return Ok(noteId);
        }

        [HttpGet("getnotes")]
        [Authorize]
        public async Task<IActionResult> GetNotesAsync()
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }
            List<NoteResponseModel> list= await noteService.GetNotesAsync(currentUserId);
            return Ok(list);
        }

        [HttpGet("getnotebyid")]
        [Authorize]
        public async Task<IActionResult> GetNoteByIdAsync(int noteId)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }
            NoteResponseModel? note= await noteService.GetNoteByIdAsync(noteId, currentUserId);
            if (note == null) return NotFound();
            return Ok(note);
        }


        [HttpPut("editnote")]
        [Authorize]
        public async Task<IActionResult> EditNoteAsync(EditNotesModel note)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }

            NoteResponseModel? updatedNote =
                await noteService.EditNoteAsync(note, currentUserId);

            if (updatedNote == null)
            {
                return NotFound();
            }

            return Ok(updatedNote);
        }

        [HttpDelete("deletenote")]
        [Authorize]
        public async Task<IActionResult> DeleteNoteAsync(int noteId)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }

            bool result =
                await noteService.DeleteNoteAsync(noteId, currentUserId);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Note deleted successfully");
        }

        [HttpPut("archunarchived")]
        [Authorize]
        public async Task<IActionResult> ArchiveUnarchiveNoteAsync(
            int noteId, bool isArchived)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }

            bool result = await noteService.ArchiveUnarchiveNoteAsync(
                noteId, currentUserId, isArchived);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Note archive status updated successfully");
        }
    }
}
