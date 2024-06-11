using OperationManagmentProject.Entites;
using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Models
{
    public class UserModel
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string IdNumber { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string? Report { get; set; }
        public bool WeaponHolder { get; set; }
        public bool Detained { get; set; }
        public bool Dead { get; set; }
        public string? BOD { get; set; }
        public ICollection<UserActionModel>? UserActions { get; set; }
        public ICollection<ImageModel>? UserImages { get; set; }
        public ICollection<UserAddressModel>? UserAddresses { get; set; }
        public ICollection<UserOrganizationModel>? UserOrganizations { get; set; }
        public UserPoliticalOrientationModel? PO { get; set; }
        public string? CreatedBy { get; set; } = "admin";
        public int? ReportActionId { get; set; }
        public int? BodGovId { get; set; }
        public string? NickName { get; set; }
    }


    public class UserActionModel
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }
        public string? ActionName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Report { get; set; }
    }
    public class ImageModel
    {
        public required string ImagePath { get; set; }
    }
    public class UserAddressModel
    {
        public int Id { get; set; }
        public bool IsHome { get; set; }
        public bool IsWork { get; set; }
        public bool IsPopular { get; set; }
        public required string Longitude { get; set; }
        public required string Latitude { get; set; }
        public string? Description { get; set; }
        public required int GovernorateId { get; set; }
        public string? GovernorateName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public ICollection<ImageModel>? AddressImages { get; set; }
        public DateTime? AddressDate { get; set; }
    }

    public class UserOrganizationModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class UserPoliticalOrientationModel
    {
        public int? PoId { get; set; }
        public string? Type { get; set; }
        public bool IsMilitary { get; set; }
        public bool IsAdvocacy { get; set; }
        public bool IsStudent { get; set; }
        public bool IsFinancial { get; set; }
        public bool IsPrisoner { get; set; }
        public bool IsDeported { get; set; }
        public int? UniversityId { get; set; }
        public string? UniversityName { get; set; }
    }
}