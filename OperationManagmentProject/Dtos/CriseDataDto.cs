using OperationManagmentProject.Models;

namespace OperationManagmentProject.Dtos
{
    public class CriseDataDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int GovernorateId { get; set; }
        public int Level { get; set; }
        public string? CriseStartDate { get; set; }
        public int PlanId { get; set; }
        public ICollection<ImageModel>? Images { get; set; }
        public ICollection<UserModel>? Users { get; set; }
        public ICollection<CriseActionDto>? Actions { get; set; }
    }
}
