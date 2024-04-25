using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class PoliticalOrientation
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string ImagePath { get; set; }
    }
}
