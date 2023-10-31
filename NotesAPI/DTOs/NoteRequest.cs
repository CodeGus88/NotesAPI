using System.ComponentModel.DataAnnotations;

namespace NotesAPI.DTOs
{
    public class NoteRequest
    {

        [Required]
        [StringLength(120)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
