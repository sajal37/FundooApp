using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        Task<NotesIdModel> AddNoteAsync(NoteCreationModel note, int userId);
        Task<List<NoteResponseModel>> GetNotesAsync(int userId);
        Task<NoteResponseModel?> GetNoteByIdAsync(int noteId, int userId);
        Task<NoteResponseModel?> EditNoteAsync(EditNotesModel note, int userId);
        Task<bool> DeleteNoteAsync(int noteId, int userId);
        Task<bool> ArchiveUnarchiveNoteAsync(int noteId, int userId, bool isArchived);
    }
}
