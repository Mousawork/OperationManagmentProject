using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class PlanPoints
    {
        [Key]
        public int Id { get; set; }
        public required int GovernorateId { get; set; }
        public required string Description { get; set; }
        public required string Latitude { get; set; }
        public required string Longitude { get; set; }
    }
}
