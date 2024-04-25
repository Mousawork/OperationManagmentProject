using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UniversityEntity
    {
        [Key]
        public int Id { get; set; }

        public required int GovernorateId { get; set; }

        public required string Name { get; set; }

        public string? ImagePath { get; set; }
    }
}
