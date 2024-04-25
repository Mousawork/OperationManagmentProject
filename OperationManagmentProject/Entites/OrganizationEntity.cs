using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class OrganizationEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int? PoId { get; set; }
        public int? GovernorateId { get; set; }
        public required string Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Report { get; set; }
        public string? CreatedBy { get; set; } = "admin";
    }
}
