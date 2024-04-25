namespace OperationManagmentProject.Models
{
    public class RegistrationModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
