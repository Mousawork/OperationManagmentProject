namespace OperationManagmentProject.Models
{
    public class AddAddressModel
    {
        public int UserId { get; set; }
        public bool IsHome { get; set; }
        public bool IsWork { get; set; }
        public bool IsPopular { get; set; }
        public string? Description { get; set; }
        public required int GovernorateId { get; set; }
        public int? CityId { get; set; }
        public required string Longitude { get; set; }
        public required string Latitude { get; set; }
        public ICollection<ImageModel>? AddressImages { get; set; }
    }
}
