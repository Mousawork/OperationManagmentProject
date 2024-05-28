
namespace OperationManagmentProject.Models
{
    public class AddUserTravelModel
    {
        public int UserId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public string? Description { get; set; }
    }
}
