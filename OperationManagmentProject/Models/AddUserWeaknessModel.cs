using OperationManagmentProject.Enums;

namespace OperationManagmentProject.Models
{
    public class AddUserWeaknessModel
    {
        public int UserId { get; set; }
        public WeaknessType WeaknessType { get; set; }
        public string? Description { get; set; }
    }
}
