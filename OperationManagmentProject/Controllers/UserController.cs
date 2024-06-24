using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Filters;
using OperationManagmentProject.Models;
using OperationManagmentProject.Services.User;

namespace OperationManagmentProject.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<string, int> _userGovernorateLookup;
        private readonly Dictionary<string, int> _userCitiesLookup;
        private readonly IUserService _userService;

        public UserController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;

            _userGovernorateLookup = _context.UserAddresses
                                    .GroupBy(d => new { d.UserId, d.GovernorateId })
                                    .Select(group => group.First())
                                    .ToDictionary(k => $"{k.UserId}~{k.GovernorateId}", v => v.GovernorateId);

            _userCitiesLookup = _context.UserAddresses
                                    .GroupBy(d => new { d.UserId, d.GovernorateId, d.CityId })
                                    .Select(group => group.First())
                                    .ToDictionary(k => $"{k.UserId}~{k.GovernorateId}~{k.CityId}", v => v.CityId ?? 0);
        }

        [HttpGet("/GetUserOrganizationRelationTypes")]
        public async Task<ActionResult<IEnumerable<UserOrganizationRelationType>>> GetUserOrganizationRelationTypes()
        {
            return await _context.UserOrganizationRelationType.ToListAsync();
        }
        [HttpGet("/GetAllUsers")]
        public IActionResult GetUsers([FromQuery] UserFilterModel filter)
        {
            try
            {
                // Start with the base query
                var query = _context.Users.AsQueryable();

                // Apply filters based on the filter model 
                if (filter.WeaponHolder == true)
                {
                    query = query.Where(u => u.WeaponHolder);
                }
                if (filter.Dead == true)
                {
                    query = query.Where(u => u.Dead);
                }
                if (filter.Detained == true)
                {
                    query = query.Where(u => u.Detained);
                }
                if (!string.IsNullOrEmpty(filter.FullName))
                {
                    query = query.Where(u => u.FullName.ToLower().Contains(filter.FullName));
                }
                if (!string.IsNullOrEmpty(filter.IdNumber))
                {
                    query = query.Where(u => u.IdNumber == filter.IdNumber);
                }
                if (!string.IsNullOrEmpty(filter.PhoneNumber))
                {
                    var userId = _context.UserPhoneNumbers.FirstOrDefault(u => u.PhoneNumber == filter.PhoneNumber)?.UserId;
                    query = query.Where(u => u.Id == userId);
                }
                if (!string.IsNullOrEmpty(filter.BOD))
                {
                    query = query.Where(u => u.BOD.Date == DateTime.Parse(filter.BOD).Date);
                }
                if (filter.ReportActionId != 0)
                {
                    query = query.Where(u => u.ReportActionId == filter.ReportActionId);
                }
                //filter createdAt
                if (!string.IsNullOrEmpty(filter.CreatedAt1) && !string.IsNullOrEmpty(filter.CreatedAt2))
                {
                    query = query.Where(u => u.CreatedAt.Date >= DateTime.Parse(filter.CreatedAt1).Date && u.CreatedAt.Date <= DateTime.Parse(filter.CreatedAt2).Date);
                    // Filter userActions array based on the actionName being true or false
                    //if (!string.IsNullOrEmpty(filter.Action)) // if the Action parameter is found in postman params
                    //{
                    //    if (filter.Action.ToLower() == "true")
                    //    {
                    //        query = query.Where(u => u.CreatedAt.Date >= DateTime.Parse(filter.CreatedAt1).Date && u.CreatedAt.Date <= DateTime.Parse(filter.CreatedAt2).Date);
                    //        //query = query.Where(u => u.UserActions.Any(a => !string.IsNullOrEmpty(a.ActionName)));
                    //    }
                    //    else if (filter.Action.ToLower() == "false")
                    //    {
                    //        query = query.Where(u => u.UserActions.All(a => !string.IsNullOrEmpty(a.ActionName)));
                    //    }
                    //    else
                    //    {
                    //        return BadRequest("Invalid value for Action field. It must be either 'true' or 'false'.");
                    //    }
                    //}
                }


                query = FilterByGovernorateAndCity(query, filter);
                query = FilterByUserPoliticalOrientation(query, filter);

                // Retrieve the filtered data
                var users = query.ToList();
                var modelToReturn = new List<UserModel>();
                foreach (var user in users)
                {
                    var usersModel = _userService.GetDetailedUserInformation(user);
                    modelToReturn.Add(usersModel);
                }

                return Ok(modelToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetUserInfoById")]
        public IActionResult GetUserInfoById(int? Id)
        {
            try
            {
                if (Id != null && Id != 0)
                {
                    var user = _context.Users.FirstOrDefault(w => w.Id == Id);
                    if (user == null) return BadRequest("User Id Invalid");
                    var userFullInfo = _userService.GetDetailedUserInformation(user);
                    return Ok(userFullInfo);
                }

                return BadRequest("User Id Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetUsersInfo")]
        public IActionResult GetUsersInfo([FromQuery] List<int> Ids)
        {
            try
            {
                var usersToReturn = new List<UserModel>();
                var users = _context.Users.Where(w => Ids.Contains(w.Id));
                foreach (var user in users)
                {
                    var userFullInfo = _userService.GetDetailedUserInformation(user);
                    usersToReturn.Add(userFullInfo);
                }
                return Ok(usersToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("AddUserWeakness")]
        public IActionResult AddUserWeakness([FromQuery] AddUserWeaknessModel model)
        {
            if (model != null)
            {
                var weaknessObject = _context.WeaknessType.FirstOrDefault(f => f.Id == model.WeaknessTypeId);
                if (weaknessObject == null)
                {
                    return BadRequest("weaknessId invalid.");
                }

                if (_context.UserWeakness.Any(u => u.UserId == model.UserId && u.WeaknessId == model.WeaknessTypeId))
                {
                    return BadRequest("User weakness already exist.");
                }


                var newUserWeakness = new UserWeakness
                {
                    UserId = model.UserId,
                    WeaknessId = model.WeaknessTypeId,
                    WeaknessName = weaknessObject.Name,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.UserWeakness.Add(newUserWeakness);
                _context.SaveChanges();

                return Ok("User weakness Added successful");
            }

            return BadRequest();
        }

        [HttpPut("UpdateUserWeakness")]
        public IActionResult UpdateUserWeakness([FromQuery] UpdateUserWeaknessModel model)
        {
            if (model != null)
            {
                var existingUserWeakness = _context.UserWeakness.FirstOrDefault(u => u.Id == model.Id);
                if (existingUserWeakness == null)
                {
                    return NotFound("User weakness not found.");
                }

                var weaknessObject = _context.WeaknessType.FirstOrDefault(f => f.Id == model.WeaknessTypeId);
                if (weaknessObject == null)
                {
                    return BadRequest("weaknessId invalid.");
                }

                existingUserWeakness.UserId = model.UserId;
                existingUserWeakness.WeaknessId = model.WeaknessTypeId;
                existingUserWeakness.WeaknessName = weaknessObject.Name;
                existingUserWeakness.Description = model.Description;
                existingUserWeakness.UpdatedAt = DateTime.UtcNow;

                _context.UserWeakness.Update(existingUserWeakness);
                _context.SaveChanges();

                return Ok("User weakness updated successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("DeleteUserWeakness/{id}")]
        public IActionResult DeleteUserWeakness(int id)
        {
            var userWeakness = _context.UserWeakness.FirstOrDefault(u => u.Id == id);
            if (userWeakness == null)
            {
                return NotFound("User weakness not found.");
            }

            _context.UserWeakness.Remove(userWeakness);
            _context.SaveChanges();

            return Ok("User weakness deleted successfully.");
        }

        [HttpGet("GetUserWeakness/{id}")]
        public IActionResult GetUserWeakness(int id)
        {
            var userWeakness = _context.UserWeakness.Where(u => u.UserId == id).ToList();
            if (userWeakness == null)
            {
                return NotFound("User weakness not found.");
            }

            return Ok(userWeakness);
        }

        //UserTravel

        [HttpPost("AddUserTravel")]
        public IActionResult AddUserTravel([FromQuery] AddUserTravelModel model)
        {
            if (model != null)
            {

                var newUserTravel = new UserTravel
                {
                    UserId = model.UserId,
                    TravelDate = model.TravelDate,
                    ReturnDate = model.ReturnDate,
                    Destination=model.Destination,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.UserTravel.Add(newUserTravel);
                _context.SaveChanges();

                return Ok("User Travel Added successful");
            }

            return BadRequest();
        }

        [HttpGet("GetUserTravel/{id}")]
        public IActionResult GetUserTravel(int id)
        {
            var userTravel = _context.UserTravel.Where(u => u.UserId == id).ToList();
            if (userTravel == null)
            {
                return NotFound("User travel not found.");
            }

            return Ok(userTravel);
        }

        [HttpDelete("DeleteUserTravel/{id}")]
        public IActionResult DeleteUserTravel(int id)
        {
            var userTravel = _context.UserTravel.FirstOrDefault(u => u.Id == id);
            if (userTravel == null)
            {
                return NotFound("User Travel not found.");
            }

            _context.UserTravel.Remove(userTravel);
            _context.SaveChanges();

            return Ok("User Travel deleted successfully.");
        }

        [HttpPut("UpdateUserTravel")]
        public IActionResult UpdateUserTravel([FromQuery] UpdateUserTravelModel model)
        {
            if (model != null)
            {
                var existingUserTravel = _context.UserTravel.FirstOrDefault(u => u.Id == model.Id);
                if (existingUserTravel == null)
                {
                    return NotFound("User Travel not found.");
                }

                existingUserTravel.UserId = model.UserId;
                existingUserTravel.TravelDate =model.TravelDate;
                existingUserTravel.ReturnDate =model.ReturnDate;
                existingUserTravel.Destination = model.Destination;
                existingUserTravel.Description = model.Description;
                //existingUserTravel.UpdatedAt = DateTime.UtcNow;

                _context.UserTravel.Update(existingUserTravel);
                _context.SaveChanges();

                return Ok("User Travel updated successfully.");
            }

            return BadRequest();
        }
        
        // todo: move it to userProfile controller
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _context.UserProfile.FirstOrDefault(f => f.Username == model.Username && f.Password == model.Password);
                if (user != null)
                    return Ok(user);

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            try
            {
                // Validate if the username is already taken
                if (_context.UserProfile.Any(u => u.Username == model.Username))
                {
                    return BadRequest("Username is already taken.");
                }

                // (future) I should hash the password before saving it to the database for security reasons.
                var newUser = new UserProfileEntity
                {
                    Username = model.Username,
                    Password = model.Password,
                    IsActive = model.IsActive,
                    Type = model.Type
                };

                _context.UserProfile.Add(newUser);
                _context.SaveChanges();

                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            try
            {
                // Validate if the username is already taken
                if (_context.Users.Any(u => u.IdNumber == model.IdNumber))
                {
                    return BadRequest("User is already exist.");
                }

                var newUser = new UserEntity
                {
                    FullName = model.FullName,
                    IdNumber = model.IdNumber,
                    Report = model.Report,
                    WeaponHolder = model.WeaponHolder,
                    Dead =model.Dead,
                    Detained =model.Detained,
                    BOD = model.BOD != null ? DateTime.Parse(model.BOD) : DateTime.MinValue,
                    CreatedBy = model.CreatedBy,
                    ReportActionId = model.ReportActionId,
                    BodGovId = model.BodGovId,
                    NickName = model.NickName
                };


                var addedEntity = _context.Users.Add(newUser);
                _context.SaveChanges();

                if (addedEntity != null)
                {
                    if (model.PhoneNumbers != null)
                    {
                        foreach (var PhoneNumber in model.PhoneNumbers)
                        {
                            var newRow = new UserPhoneNumber
                            {
                                UserId = addedEntity.Entity.Id,
                                PhoneNumber = PhoneNumber
                            };
                            var phoneAddedEntity = _context.UserPhoneNumbers.Add(newRow);
                            _context.SaveChanges();
                        }
                    }

                    if (model.UserAddresses != null)
                    {
                        foreach (var address in model.UserAddresses)
                        {
                            var newAddress = new UserAddressEntity
                            {
                                UserId = addedEntity.Entity.Id,
                                IsHome = address.IsHome,
                                IsWork = address.IsWork,
                                IsPopular = address.IsPopular,
                                Description = address.Description,
                                GovernorateId = address.GovernorateId,
                                CityId = address.CityId,
                                Longitude = address.Longitude,
                                Latitude = address.Latitude,
                                AddressDate = address.AddressDate,
                            };
                            var addressAddedEntity = _context.UserAddresses.Add(newAddress);
                            _context.SaveChanges();

                            if (addressAddedEntity != null)
                            {
                                if (address.AddressImages != null)
                                {
                                    foreach (var image in address.AddressImages)
                                    {
                                        var newImage = new AddressImageEntity
                                        {
                                            AddressId = addressAddedEntity.Entity.AddressId,
                                            ImagePath = image.ImagePath
                                        };
                                        _context.AddressImages.Add(newImage);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    if (model.UserImages != null)
                    {
                        foreach (var image in model.UserImages)
                        {
                            var newImage = new UserImageEntity
                            {
                                UserId = addedEntity.Entity.Id,
                                ImagePath = image.ImagePath
                            };
                            _context.UserImages.Add(newImage);
                            _context.SaveChanges();
                        }
                    }

                    if (model.UserActions != null)
                    {
                        foreach (var action in model.UserActions)
                        {
                            var newAction = new UserActionEntity
                            {
                                UserId = addedEntity.Entity.Id,
                                ActionId = action.ActionId,
                                Report = action.Report,
                            };
                            _context.UserActions.Add(newAction);
                            _context.SaveChanges();
                        }

                    }

                    if (model.PO != null && model.PO?.PoId != null)
                    {
                        var newPo = new UserPoliticalOrientationEntity
                        {
                            PoId = model.PO.PoId.Value,
                            UserId = addedEntity.Entity.Id,
                            IsAdvocacy = model.PO.IsAdvocacy,
                            IsStudent = model.PO.IsStudent,
                            IsPrisoner = model.PO.IsPrisoner,
                            IsMilitary = model.PO.IsMilitary,
                            IsDeported = model.PO.IsDeported,
                            IsFinancial = model.PO.IsFinancial,
                            UniversityId = model.PO.UniversityId,
                        };
                        _context.UserPoliticalOrientation.Add(newPo);
                        _context.SaveChanges();
                    }

                }
                return Ok(addedEntity?.Entity.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPatch("UpdateUser/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserModel model)
        {
            try
            {
                if (model == null)
                {
                    return NotFound("Invalid Model");
                }

                // Find the user to update
                var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (userToUpdate == null)
                {
                    return NotFound("User not found");
                }

                // Update user properties
                if (model.FullName != null)
                    userToUpdate.FullName = model.FullName;

                if (model.NickName != null)
                    userToUpdate.NickName = model.NickName;

                if (model.PhoneNumbers != null)
                {
                    var existingRecords = _context.UserPhoneNumbers.Where(w => w.UserId == userId);
                    _context.UserPhoneNumbers.RemoveRange(existingRecords);
                    _context.SaveChanges();

                    foreach (var phone in model.PhoneNumbers)
                    {
                        _context.UserPhoneNumbers.Add(new UserPhoneNumber { PhoneNumber = phone, UserId = userId });
                        _context.SaveChanges();
                    }
                }

                if (model.Report != null)
                    userToUpdate.Report = model.Report;

                if (model.Report != null)
                    userToUpdate.WeaponHolder = model.WeaponHolder;

                if (model.Report != null)
                    userToUpdate.Detained = model.Detained;

                if (model.Report != null)
                    userToUpdate.Dead = model.Dead;

                if (model.BOD != null)
                    userToUpdate.BOD = model.BOD != null ? DateTime.Parse(model.BOD) : DateTime.MinValue;

                // Update user addresses
                if (model.UserAddresses != null)
                {
                    // Remove existing addresses
                    var existingAddresses = _context.UserAddresses.Where(ua => ua.UserId == userId);
                    _context.UserAddresses.RemoveRange(existingAddresses);

                    // Add new addresses
                    foreach (var address in model.UserAddresses)
                    {
                        var newAddress = new UserAddressEntity
                        {
                            UserId = userId,
                            IsHome = address.IsHome,
                            IsWork = address.IsWork,
                            IsPopular = address.IsPopular,
                            Description = address.Description,
                            GovernorateId = address.GovernorateId,
                            CityId = address.CityId,
                            Longitude = address.Longitude,
                            Latitude = address.Latitude,
                            AddressDate = address.AddressDate,
                        };
                        var addressAddedEntity = _context.UserAddresses.Add(newAddress);
                        _context.SaveChanges();

                        if (addressAddedEntity != null)
                        {
                            if (address.AddressImages != null)
                            {
                                foreach (var image in address.AddressImages)
                                {
                                    var newImage = new AddressImageEntity
                                    {
                                        AddressId = addressAddedEntity.Entity.AddressId,
                                        ImagePath = image.ImagePath
                                    };
                                    _context.AddressImages.Add(newImage);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }

                // Update user images
                if (model.UserImages != null)
                {
                    // Remove existing images
                    var existingImages = _context.UserImages.Where(ui => ui.UserId == userId);
                    _context.UserImages.RemoveRange(existingImages);

                    // Add new images
                    foreach (var image in model.UserImages)
                    {
                        var newImage = new UserImageEntity
                        {
                            UserId = userId,
                            ImagePath = image.ImagePath
                        };
                        _context.UserImages.Add(newImage);
                        _context.SaveChanges();
                    }
                }

                // Update user actions
                if (model.UserActions != null)
                {
                    // Remove existing actions
                    var existingActions = _context.UserActions.Where(ua => ua.UserId == userId);
                    _context.UserActions.RemoveRange(existingActions);

                    // Add new actions
                    foreach (var action in model.UserActions)
                    {
                        var newAction = new UserActionEntity
                        {
                            UserId = userId,
                            ActionId = action.ActionId,
                            Report = action.Report,
                        };
                        _context.UserActions.Add(newAction);
                        _context.SaveChanges();
                    }
                }

                // Update user Political Orientation
                if (model.PO != null && model.PO.PoId != null)
                {
                    // Remove existing actions
                    var existingPos = _context.UserPoliticalOrientation.Where(ua => ua.UserId == userId);
                    _context.UserPoliticalOrientation.RemoveRange(existingPos);

                    var newPo = new UserPoliticalOrientationEntity
                    {
                        PoId = model.PO.PoId.Value,
                        UserId = userId,
                        IsAdvocacy = model.PO.IsAdvocacy,
                        IsStudent = model.PO.IsStudent,
                        IsPrisoner = model.PO.IsPrisoner,
                        IsMilitary = model.PO.IsMilitary,
                        IsDeported = model.PO.IsDeported,
                        IsFinancial = model.PO.IsFinancial,
                        UniversityId = model.PO.UniversityId,
                    };
                    _context.UserPoliticalOrientation.Add(newPo);
                    _context.SaveChanges();
                }

                // Save changes to the database
                _context.SaveChanges();

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetUsersNotAssociatedToOrganization/{organizationId}")]
        public IActionResult GetUsersNotAssociatedToOrganization(int organizationId)
        {
            try
            {
                var assignedUserIds = _context.UserOrganization.Where(w => w.OrganizationId == organizationId).Select(s => s.UserId).ToList();
                var usersInfo = _context.Users.Select(s => new { s.Id, s.FullName }).Where(w => !assignedUserIds.Contains(w.Id));

                return Ok(usersInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        private IQueryable<UserEntity> FilterByUserPoliticalOrientation(IQueryable<UserEntity> query, UserFilterModel filter)
        {
            // filter based on the UserPoliticalOrientation
            if (!string.IsNullOrEmpty(filter.PoId.ToString()) && filter.PoId != 0)
            {
                query = query.Where(u => _context.UserPoliticalOrientation.Any(po =>
                            po.UserId == u.Id &&
                            po.PoId == filter.PoId &&
                            (filter.IsMilitary == false || po.IsMilitary == filter.IsMilitary) &&
                            (filter.IsAdvocacy == false || po.IsAdvocacy == filter.IsAdvocacy) &&
                            (filter.IsStudent == false || po.IsStudent == filter.IsStudent) &&
                            (filter.IsFinancial == false || po.IsFinancial == filter.IsFinancial) &&
                            (filter.IsPrisoner == false || po.IsPrisoner == filter.IsPrisoner) &&
                            (filter.IsDeported == false || po.IsDeported == filter.IsDeported))
                        );
            }

            return query;
        }
        private IQueryable<UserEntity> FilterByGovernorateAndCity(IQueryable<UserEntity> query, UserFilterModel filter)
        {
            // filter based on the Governorate and city
            if (!string.IsNullOrEmpty(filter.GovernorateId.ToString()) && filter.GovernorateId != 0)
            {
                // Materialize the data before applying the filter
                var userGovernorateLookup = _userGovernorateLookup;

                query = query.AsEnumerable()
                    .Where(u => userGovernorateLookup.ContainsKey($"{u.Id}~{filter.GovernorateId}") && userGovernorateLookup[$"{u.Id}~{filter.GovernorateId}"] == filter.GovernorateId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.CityId.ToString()) && filter.CityId != 0)
                {
                    query = query.AsEnumerable()
                        .Where(u => _userCitiesLookup.ContainsKey($"{u.Id}~{filter.GovernorateId}~{filter.CityId}") && _userCitiesLookup[$"{u.Id}~{filter.GovernorateId}~{filter.CityId}"] == filter.CityId)
                        .AsQueryable();
                }
            }
            return query;
        }
    }
}