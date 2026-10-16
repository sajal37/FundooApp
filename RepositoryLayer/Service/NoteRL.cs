
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext db;

        public NoteRL(FundooContext db)
        {
            this.db = db;
        }

        public async Task<NoteEntity> AddNoteAsync(NoteEntity note)
        {
            db.Notes.Add(note);
            await db.SaveChangesAsync();
            return note;
        }

        public async Task<List<NoteEntity>> GetNotesAsync(int userId)
        {
            return await db.Notes
                .Where(note => note.UserId == userId && !note.IsDeleted)
                .ToListAsync();
        }

        public async Task<NoteEntity?> GetNoteByIdAsync(int noteId, int userId)
        {
            return await db.Notes.FirstOrDefaultAsync(
                note => note.NoteId == noteId &&
                        note.UserId == userId &&
                        !note.IsDeleted);
        }

        public async Task<NoteEntity?> EditNoteAsync(
            int noteId, int userId, NoteEntity newNote)
        {
            NoteEntity? note = await db.Notes.FirstOrDefaultAsync(
                n => n.NoteId == noteId &&
                     n.UserId == userId &&
                     !n.IsDeleted);

            if (note == null)
            {
                return null;
            }

            note.Title = newNote.Title;
            note.Description = newNote.Description;
            note.Colour = newNote.Colour;

            await db.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteNoteAsync(int noteId, int userId)
        {
            NoteEntity? note = await db.Notes.FirstOrDefaultAsync(
                n => n.NoteId == noteId &&
                     n.UserId == userId &&
                     !n.IsDeleted);

            if (note == null)
            {
                return false;
            }

            note.IsDeleted = true;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ArchiveUnarchiveNoteAsync(
            int noteId, int userId, bool isArchived)
        {
            NoteEntity? note = await db.Notes.FirstOrDefaultAsync(
                n => n.NoteId == noteId &&
                     n.UserId == userId &&
                     !n.IsDeleted);

            if (note == null)
            {
                return false;
            }

            note.IsArchived = isArchived;
            await db.SaveChangesAsync();

            return true;
        }
    }
}
