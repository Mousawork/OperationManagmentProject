using OperationManagmentProject.Enums;

namespace OperationManagmentProject.Models
{
    public class AddUserWeaknessModel
    {
        public int UserId { get; set; }
        public int WeaknessTypeId { get; set; }
        public string? Description { get; set; }
    }
}
