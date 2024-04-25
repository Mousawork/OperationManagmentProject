using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class Governorate
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public required string Name { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
