using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string IdNumber { get; set; }
        public string? Report { get; set; }
        public bool WeaponHolder { get; set; }
        public bool Detained { get; set; }
        public bool Dead { get; set; }
        public DateTime BOD { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ReportActionId { get; set; }
        public int? BodGovId { get; set; }
        public string? NickName { get; set; }
    }
}