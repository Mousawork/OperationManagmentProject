namespace OperationManagmentProject.Dtos
{
    public class CriseActionDto
    {
        public int Id { get; set; }
        public int CriseId { get; set; }
        public int ActionId { get; set; }
        public string? ActionName { get; set; }
        public string? Report { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
