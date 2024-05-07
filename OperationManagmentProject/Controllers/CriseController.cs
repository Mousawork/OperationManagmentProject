using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Dtos;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Filters;
using OperationManagmentProject.Models;
using OperationManagmentProject.Services.User;

namespace OperationManagmentProject.Controllers
{
    public class CriseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        public CriseController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;

        }

        [HttpPost("CreateCrise")]
        public IActionResult CreateCrise([FromBody] AddCriseModel model)
        {
            if (model != null)
            {
                var newUserCrise = new CriseEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    GovernorateId = model.GovernorateId,
                    Level = model.Level,
                    PlanId = model.PlanId,
                    CriseStartDate = model.CriseStartDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var addedEntity = _context.Crises.Add(newUserCrise);
                _context.SaveChanges();

                if (addedEntity != null)
                {
                    if (model.Images != null)
                    {
                        foreach (var image in model.Images)
                        {
                            var newImage = new CriseImageEntity
                            {
                                CriseId = addedEntity.Entity.Id,
                                ImagePath = image.ImagePath
                            };
                            _context.CrisesImages.Add(newImage);
                            _context.SaveChanges();
                        }
                    }

                    if (model.UserIds != null)
                    {
                        foreach (var userId in model.UserIds)
                        {
                            var newUser = new CriseUserEntity
                            {
                                CriseId = addedEntity.Entity.Id,
                                UserId = userId,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            _context.CrisesUsers.Add(newUser);
                            _context.SaveChanges();
                        }
                    }
                }

                return Ok(addedEntity.Entity.Id);
            }

            return BadRequest();
        }


        [HttpPost("UpdateCrise/{id}")]
        public IActionResult UpdateCrise(int id, [FromBody] UpdateCriseModel model)
        {
            var existingCrise = _context.Crises.FirstOrDefault(c => c.Id == id);

            if (existingCrise == null)
            {
                return NotFound("Crise not found");
            }

            // Update existing crisis entity with new information
            existingCrise.Name = model.Name ?? existingCrise.Name;
            existingCrise.Description = model.Description ?? existingCrise.Description;
            existingCrise.GovernorateId = model.GovernorateId;
            existingCrise.Level = model.Level;
            existingCrise.PlanId = model.PlanId;
            existingCrise.CriseStartDate = model.CriseStartDate ?? existingCrise.CriseStartDate;
            existingCrise.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            // Update associated images
            if (model.Images != null)
            {
                // Clear existing images
                var existingImages = _context.CrisesImages.Where(ci => ci.CriseId == id);
                _context.CrisesImages.RemoveRange(existingImages);

                // Add new images
                foreach (var image in model.Images)
                {
                    var newImage = new CriseImageEntity
                    {
                        CriseId = id,
                        ImagePath = image.ImagePath
                    };
                    _context.CrisesImages.Add(newImage);
                }
                _context.SaveChanges();
            }

            // Update associated users
            if (model.UserIds != null)
            {
                // Clear existing users
                var existingUsers = _context.CrisesUsers.Where(cu => cu.CriseId == id);
                _context.CrisesUsers.RemoveRange(existingUsers);

                // Add new users
                foreach (var userId in model.UserIds)
                {
                    var newUser = new CriseUserEntity
                    {
                        CriseId = id,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.CrisesUsers.Add(newUser);
                }
                _context.SaveChanges();
            }

            // Update associated actions
            if (model.ActionIds != null)
            {
                // Clear existing actions
                var existingActions = _context.CrisesActions.Where(ca => ca.CriseId == id);
                _context.CrisesActions.RemoveRange(existingActions);

                // Add new actions
                foreach (var actionId in model.ActionIds)
                {
                    var newAction = new CriseActionEntity
                    {
                        CriseId = id,
                        ActionId = actionId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.CrisesActions.Add(newAction);
                }
                _context.SaveChanges();
            }

            return Ok("Crise updated successfully");
        }


        [HttpGet("GetCrises")]
        public IActionResult GetCrises([FromQuery] CriseFilterModel filter)
        {
            try
            {
                // Start with the base query
                var query = _context.Crises.AsQueryable();
                var CrisesDto = new List<CriseDataDto>();

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(u => u.Name.ToLower().Contains(filter.Name));
                }
                if (!string.IsNullOrEmpty(filter.CriseStartDate))
                {
                    query = query.Where(u => u.CriseStartDate == filter.CriseStartDate);
                }
                if (filter.GovernorateId != 0)
                {
                    query = query.Where(u => u.GovernorateId == filter.GovernorateId);
                }
                // Retrieve the filtered data
                var CrisesList = query.ToList();


                foreach (var item in CrisesList)
                {
                    CrisesDto.Add(new CriseDataDto
                    {
                        Id = item.Id,
                        CriseStartDate = item.CriseStartDate,
                        Description = item.Description,
                        Name = item.Name,
                        PlanId = item.PlanId,
                        GovernorateId = item.GovernorateId,
                        Level = item.Level,
                        Users = GetUsersInfo(item.Id),
                        Images = GetCriseImages(item.Id),
                        Actions = GetCriseActions(item.Id)
                    });
                }
                return Ok(CrisesDto);



            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        private ICollection<CriseActionDto>? GetCriseActions(int id)
        {
            var actions = _context.CrisesActions.Where(w => w.CriseId == id).ToList();
            if (!actions.Any()) return null;

            var dtoToReturn = new List<CriseActionDto>();
            var actionNames = _context.Action.ToDictionary(k => k.Id, v => v.Name);

            foreach (var action in actions)
            {
                dtoToReturn.Add(new CriseActionDto
                {
                    Id = action.Id,
                    CriseId = action.CriseId,
                    ActionId = action.ActionId,
                    ActionName = actionNames.ContainsKey(action.ActionId) ? actionNames[action.ActionId] : "",
                    Report = action.Report,
                    CreatedAt = action.CreatedAt,
                    UpdatedAt = action.UpdatedAt,
                });
            }
            return dtoToReturn;
        }

        private ICollection<ImageModel>? GetCriseImages(int id)
        {
            var imagePaths = _context.CrisesImages.Where(w => w.CriseId == id).Select(s => s.ImagePath).ToList();
            if (!imagePaths.Any()) return null;

            var ImagesToReturn = new List<ImageModel>();
            foreach (var path in imagePaths)
            {
                ImagesToReturn.Add(new ImageModel { ImagePath = path });
            }
            return ImagesToReturn;
        }

        private ICollection<UserModel>? GetUsersInfo(int id)
        {
            var userIds = _context.CrisesUsers.Where(w => w.CriseId == id).Select(s => s.UserId).ToList();
            if (!userIds.Any()) return null;

            var usersToReturn = new List<UserModel>();
            var users = _context.Users.Where(w => userIds.Contains(w.Id));
            foreach (var user in users)
            {
                var userFullInfo = _userService.GetDetailedUserInformation(user);
                usersToReturn.Add(userFullInfo);
            }
            return usersToReturn;
        }
    }
}
