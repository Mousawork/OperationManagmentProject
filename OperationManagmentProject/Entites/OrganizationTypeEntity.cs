using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class OrganizationTypeEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
