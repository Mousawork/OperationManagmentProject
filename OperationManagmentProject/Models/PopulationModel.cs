namespace OperationManagmentProject.Models
{
    public class PopulationModel
    {
        public string? Identity { get; set; }
        public required string FName { get; set; }
        public string? SName { get; set; }
        public string? TName { get; set; }
        public required string LName { get; set; }
        public string? MName { get; set; }
        public string? BirthDate { get; set; }
        public string? Governorate { get; set; }
        public string? Neighborhood { get; set; }
    }
}
