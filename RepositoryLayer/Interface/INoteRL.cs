
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task<NoteEntity> AddNoteAsync(NoteEntity note);
        Task<List<NoteEntity>> GetNotesAsync(int userId);
        Task<NoteEntity?> GetNoteByIdAsync(int noteId, int userId);
        Task<NoteEntity?> EditNoteAsync(int noteId, int userId, NoteEntity newNote);
        Task<bool> DeleteNoteAsync(int noteId, int userId);
        Task<bool> ArchiveUnarchiveNoteAsync(int noteId, int userId, bool isArchived);
    }
}
