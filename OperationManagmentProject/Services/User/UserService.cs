using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<int, string> _locationsLookup;
        private readonly Dictionary<int, string> _politicalOrientationTypeLookup;
        private readonly Dictionary<int, string> _userActionsLookup;

        public UserService(AppDbContext context)
        {
            _context = context;
            _locationsLookup = _context.Governorate
                            .Select(s => new { s.Id, s.Name })
                            .Distinct()
                            .ToDictionary(key => key.Id, value => value.Name);

            _politicalOrientationTypeLookup = _context.PoliticalOrientationType.Distinct().ToDictionary(k => k.Id, v => v.Type);

            _userActionsLookup = _context.Action.ToDictionary(k => k.Id, v => v.Name);
        }

        public UserModel GetDetailedUserInformation(UserEntity user)
        {
            return new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                IdNumber = user.IdNumber,
                PhoneNumbers = _context.UserPhoneNumbers.Where(w => w.UserId == user.Id).Select(s => s.PhoneNumber).ToList(),
                Report = user.Report,
                WeaponHolder = user.WeaponHolder,
                BOD = user.BOD.ToString(),
                UserImages = GetUserImages(user.Id),
                UserActions = GetUserActions(user.Id),
                UserAddresses = GetUserAddresses(user.Id),
                UserOrganizations = GetUserOrganizations(user.Id),
                PO = GetUserPoliticalOrientation(user.Id),
                CreatedBy = user.CreatedBy,
                ReportActionId = user.ReportActionId,
                BodGovId = user.BodGovId,
                NickName = user.NickName
            };
        }

        private ICollection<ImageModel>? GetUserImages(int id)
        {
            return _context.UserImages.Where(w => w.UserId == id)?.Select(x => new ImageModel { ImagePath = x.ImagePath }).ToList();
        }

        private ICollection<UserActionModel>? GetUserActions(int id)
        {
            var userActions = _context.UserActions.Where(w => w.UserId == id).ToList();
            var model = userActions?.Select(x => new UserActionModel
            {
                Id = x.Id,
                ActionId = x.ActionId,
                UserId = id,
                ActionName = GetActionName(x.ActionId),
                CreatedAt = x.CreatedAt,
                Report = x.Report,
            }).ToList();

            return model;
        }

        private ICollection<UserAddressModel>? GetUserAddresses(int id)
        {
            return _context.UserAddresses.Where(w => w.UserId == id)?.Select(x => new UserAddressModel
            {
                Id = x.AddressId,
                IsHome = x.IsHome,
                IsWork = x.IsWork,
                IsPopular = x.IsPopular,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                AddressImages = _context.AddressImages.Where(w => w.AddressId == x.AddressId).Select(x => new ImageModel { ImagePath = x.ImagePath }).ToList(),
                GovernorateId = x.GovernorateId,
                GovernorateName = _locationsLookup.ContainsKey(x.GovernorateId) ? _locationsLookup[x.GovernorateId] : string.Empty,
                CityId = x.CityId,
                CityName = (x.CityId != null && _locationsLookup.ContainsKey(x.CityId.Value)) ? _locationsLookup[x.CityId.Value] : string.Empty,
                Description = x.Description,
                AddressDate = x.AddressDate,
            }).ToList();
        }

        private ICollection<UserOrganizationModel> GetUserOrganizations(int id)
        {
            var userOrganization = _context.UserOrganization.Where(w => w.UserId == id).ToList();

            var model = userOrganization.Select(s => new UserOrganizationModel
            {
                Id = s.OrganizationId,
                Name = GetOrganizationName(s.OrganizationId)
            }).ToList();
            return model;
        }

        private UserPoliticalOrientationModel? GetUserPoliticalOrientation(int id)
        {
            var entity = _context.UserPoliticalOrientation.Where(u => id == u.UserId).FirstOrDefault();

            if (entity == null || !_politicalOrientationTypeLookup.ContainsKey(entity.PoId))
            {
                return null;
            }

            return new UserPoliticalOrientationModel
            {
                PoId = entity.PoId,
                Type = _politicalOrientationTypeLookup[entity.PoId],
                IsMilitary = entity.IsMilitary,
                IsAdvocacy = entity.IsAdvocacy,
                IsStudent = entity.IsStudent,
                IsFinancial = entity.IsFinancial,
                IsPrisoner = entity.IsPrisoner,
                IsDeported = entity.IsDeported,
                UniversityId = entity.UniversityId,
                UniversityName = _context.University.FirstOrDefault(w => w.Id == entity.UniversityId)?.Name
            };
        }

        private string? GetActionName(int id)
        {
            return _userActionsLookup.ContainsKey(id) ? _userActionsLookup[id] : "اجراء تم اتخاذه";
        }

        private string? GetOrganizationName(int organizationId)
        {
            return _context.Organization.FirstOrDefault(f => f.Id == organizationId)?.Name;
        }
    }
}
