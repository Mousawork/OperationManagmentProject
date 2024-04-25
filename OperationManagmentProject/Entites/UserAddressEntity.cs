using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserAddressEntity
    {
        [Key] // This attribute marks the property as the primary key
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public bool IsHome { get; set; }
        public bool IsWork { get; set; }
        public bool IsPopular { get; set; }
        public string? Description { get; set; }
        public required int GovernorateId { get; set; }
        public int? CityId { get; set; }
        public required string Longitude { get; set; }
        public required string Latitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? AddressDate { get; set; }
    }
}
