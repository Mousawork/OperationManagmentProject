using OperationManagmentProject.Enums;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Models
{
    public class UpdateUserWeaknessModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WeaknessTypeId { get; set; }

        public string? Description { get; set; }
    }
}
