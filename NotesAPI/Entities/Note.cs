using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAPI.Entities
{
    [Table("Notes")]
    public class Note
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
