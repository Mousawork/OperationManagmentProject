using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class CriseEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CriseStartDate { get; set; }
        public int PlanId { get; set; }
        public int GovernorateId { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

  