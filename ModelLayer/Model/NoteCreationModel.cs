using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class NoteCreationModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
    }
}
