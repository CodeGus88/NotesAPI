using System.ComponentModel.DataAnnotations;

namespace NotesAPI.Entities
{
    public class EntityBase<IdType>
    {
        [Key]
        public IdType Id { get; set; }
    }
}
