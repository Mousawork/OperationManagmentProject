namespace OperationManagmentProject.Models
{
    public class AddOrganizationModel
    {
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
