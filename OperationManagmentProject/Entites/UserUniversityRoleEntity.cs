using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserUniversityRoleEntity
    {

        [Key]
        public int Id { get; set; }
        public required int UniversityId { get; set; }
        public required int UserId { get; set; }
        public required int UniversityStudentArmId { get; set; }
        public required int RoleId { get; set; }
        public required string Name { get; set; }
    }
}
