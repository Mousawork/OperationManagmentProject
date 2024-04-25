namespace OperationManagmentProject.Models
{
    public class CreateCriseReportModel
    {
        public int CriseId { get; set; }
        public string? Report { get; set; }
        public string? SessionNumber { get; set; }
        public string? InformationSource { get; set; }
    }
}
