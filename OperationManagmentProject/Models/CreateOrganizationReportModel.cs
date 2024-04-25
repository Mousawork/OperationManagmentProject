namespace OperationManagmentProject.Models
{
    public class CreateOrganizationReportModel
    {
        public int OrganizationId { get; set; }
        public string? Report { get; set; }
        public string? SessionNumber { get; set; }
        public string? InformationSource { get; set; }
    }
}
