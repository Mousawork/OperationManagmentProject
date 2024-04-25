using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserPoliticalOrientationEntity
    {
        [Key]
        public int Id { get; set; }
        public required int PoId { get; set; }
        public required int UserId { get; set; }
        public bool IsMilitary { get; set; }
        public bool IsAdvocacy { get; set; }
        public bool IsStudent { get; set; }
        public bool IsFinancial { get; set; }
        public bool IsPrisoner { get; set; }
        public bool IsDeported { get; set; }
        public int? UniversityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
