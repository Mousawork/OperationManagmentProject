namespace OperationManagmentProject.Models
{
    public class AssignUserToStudentArmModel
    {
        public required int UniversityId { get; set; }
        public required int UserId { get; set; }
        public required int UniversityStudentArmId { get; set; }
        public required int RoleId { get; set; }
        public required string Name { get; set; }
    }
}
