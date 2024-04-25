using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class Session
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
