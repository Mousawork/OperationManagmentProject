using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UniversityStudentArm
    {
        [Key]
        public int Id { get; set; }
        public required int UniversityId { get; set; }
        public required int PoId { get; set; }
        public required string Name { get; set; }
        public string? ImagePath { get; set; }
    }
}
