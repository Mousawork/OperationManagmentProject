using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class OverlayPlan
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int PlanId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
    }
}
