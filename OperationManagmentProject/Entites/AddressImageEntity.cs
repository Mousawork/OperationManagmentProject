using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class AddressImageEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int ImageId { get; set; }
        public int AddressId { get; set; }
        public required string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
