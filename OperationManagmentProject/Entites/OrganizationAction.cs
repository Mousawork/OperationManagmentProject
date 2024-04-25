using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationManagmentProject.Entites
{
    [Table("OrganizationActions")]
    public class OrganizationAction
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int OrganizationId { get; set; }
        public string? Report { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
