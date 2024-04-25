namespace OperationManagmentProject.Models
{
    public class CreateUserReportModel
    {
        public int UserId { get; set; }
        public string? Report { get; set; }
        public string? SessionNumber { get; set; }
        public string? InformationSource { get; set; }
    }
}
