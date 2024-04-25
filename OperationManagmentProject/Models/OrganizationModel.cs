namespace OperationManagmentProject.Models
{
    public class OrganizationModel
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName { get; set; }
        public int? PoId { get; set; }
        public string? PoName { get; set; }
        public string? Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Report { get; set; }
        public string? CreatedBy { get; set; } = "admin";
        public List<OrganizationRelatedUserModel>? RelatedUser { get; set; }
    }
}
