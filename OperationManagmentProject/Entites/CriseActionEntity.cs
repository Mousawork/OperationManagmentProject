using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class CriseActionEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int CriseId { get; set; }
        public int ActionId { get; set; }
        public string? Report { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}