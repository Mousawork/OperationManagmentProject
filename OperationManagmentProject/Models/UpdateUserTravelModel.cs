using System.ComponentModel.DataAnnotations;
namespace OperationManagmentProject.Models
{
    public class UpdateUserTravelModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime TravelDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public string? Description { get; set; }
    }
}
