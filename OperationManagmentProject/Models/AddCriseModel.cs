namespace OperationManagmentProject.Models
{
    public class AddCriseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public required int GovernorateId { get; set; }
        public int Level { get; set; }
        public string? CriseStartDate { get; set; }
        public int PlanId { get; set; }
        public ICollection<ImageModel>? Images { get; set; }
        public ICollection<int>? UserIds { get; set; }
    }
}
