using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserImageEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int ImageId { get; set; }
        public int UserId { get; set; }
        public required string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
