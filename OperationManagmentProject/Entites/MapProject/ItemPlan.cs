using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class ItemPlan
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int IconId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Iconic { get; set; } = false;
    }
}
