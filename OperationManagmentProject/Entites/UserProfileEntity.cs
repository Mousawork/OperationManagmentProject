using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    // This class identify the users who is authinticated to login 
    public class UserProfileEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}