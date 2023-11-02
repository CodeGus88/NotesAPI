using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAPI.Entities
{
    [Table("Notes")]
    public class Note: EntityBase<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
