using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class PolygonData
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int PlanId { get; set; }
        public required string Color { get; set; }
    }
}
