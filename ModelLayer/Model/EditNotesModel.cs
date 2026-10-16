using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class EditNotesModel
    {
        [Required]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
    }
}
