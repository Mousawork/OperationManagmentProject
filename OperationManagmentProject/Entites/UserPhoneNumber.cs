using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserPhoneNumber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
