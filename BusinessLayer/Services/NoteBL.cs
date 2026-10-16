
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL noteRepository;

        public NoteBL(INoteRL noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<NotesIdModel> AddNoteAsync(
            NoteCreationModel note, int userId)
        {
            NoteEntity nt = new NoteEntity();
            nt.Title = note.Title;
            nt.Description = note.Description;
            nt.Colour = note.Colour;
            nt.IsArchived = false;
            nt.IsDeleted = false;
            nt.UserId = userId;

            NoteEntity ntNew = await noteRepository.AddNoteAsync(nt);

            NotesIdModel noteId = new NotesIdModel();
            noteId.NoteId = ntNew.NoteId;

            return noteId;
        }

        public async Task<List<NoteResponseModel>> GetNotesAsync(int userId)
        {
            List<NoteEntity> list =
                await noteRepository.GetNotesAsync(userId);

            List<NoteResponseModel> res = new List<NoteResponseModel>();

            for (int i = 0; i < list.Count; i++)
            {
                NoteResponseModel note = new NoteResponseModel();
                note.NoteId = list[i].NoteId;
                note.Title = list[i].Title;
                note.Description = list[i].Description;
                note.Colour = list[i].Colour;

                res.Add(note);
            }

            return res;
        }

        public async Task<NoteResponseModel?> GetNoteByIdAsync(
            int noteId, int userId)
        {
            NoteEntity? note =
                await noteRepository.GetNoteByIdAsync(noteId, userId);

            if (note == null)
            {
                return null;
            }

            NoteResponseModel res = new NoteResponseModel();
            res.NoteId = note.NoteId;
            res.Title = note.Title;
            res.Description = note.Description;
            res.Colour = note.Colour;

            return res;
        }

        public async Task<NoteResponseModel?> EditNoteAsync(
            EditNotesModel newNote, int userId)
        {
            NoteEntity noteToUpdate = new NoteEntity();
            noteToUpdate.Title = newNote.Title;
            noteToUpdate.Description = newNote.Description;
            noteToUpdate.Colour = newNote.Colour;

            NoteEntity? note = await noteRepository.EditNoteAsync(
                newNote.NoteId, userId, noteToUpdate);

            if (note == null)
            {
                return null;
            }

            NoteResponseModel res = new NoteResponseModel();
            res.NoteId = note.NoteId;
            res.Title = note.Title;
            res.Description = note.Description;
            res.Colour = note.Colour;

            return res;
        }

        public async Task<bool> DeleteNoteAsync(int noteId, int userId)
        {
            return await noteRepository.DeleteNoteAsync(noteId, userId);
        }

        public async Task<bool> ArchiveUnarchiveNoteAsync(
            int noteId, int userId, bool isArchived)
        {
            return await noteRepository.ArchiveUnarchiveNoteAsync(
                noteId, userId, isArchived);
        }
    }
}
