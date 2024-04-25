namespace OperationManagmentProject.Filters
{
    public class UserFilterModel
    {
        public int GovernorateId { get; set; }
        public int CityId { get; set; }
        public bool WeaponHolder { get; set; }

        public int PoId { get; set; }
        public bool IsMilitary { get; set; }
        public bool IsAdvocacy { get; set; }
        public bool IsStudent { get; set; }
        public bool IsFinancial { get; set; }
        public bool IsPrisoner { get; set; }
        public bool IsDeported { get; set; }
        public string? FullName { get; set; }
        public string? IdNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BOD { get; set; }
        public int ReportActionId { get; set; }
        public string? CreatedAt1 { get; set; }
        public string? CreatedAt2 { get; set; }
        public string Action { get; set; }
    }
}
