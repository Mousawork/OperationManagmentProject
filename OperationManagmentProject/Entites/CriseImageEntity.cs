using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class CriseImageEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int CriseId { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}