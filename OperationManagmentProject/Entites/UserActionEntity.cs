using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserActionEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Report { get; set; }
    }
}
