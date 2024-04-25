using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Enums;
using OperationManagmentProject.Models;
using OperationManagmentProject.Services.User;

namespace OperationManagmentProject.Controllers
{
    public class UniversityController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public UniversityController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("/GetAllUniversities")]
        public IActionResult GetAllUniversities()
        {
            var universites = _context.University.ToList();
            return Ok(universites);
        }

        [HttpGet("/GetUniversityByGovernorateId/{governorateId}")]
        public IActionResult GetUniversityByGovernorateId(int governorateId)
        {
            var universites = _context.University.Where(w => w.GovernorateId == governorateId).ToList();
            return Ok(universites);
        }

        [HttpGet("/GetUniversityStudentArm")]
        public IActionResult GetUniversityStudentArm(int universityId, int poId)
        {
            var arms = _context.UniversityStudentArm.Where(w => w.UniversityId == universityId && w.PoId == poId).ToList();
            return Ok(arms);
        }

        [HttpGet("/GetUniversityUserRoles")]
        public IActionResult GetUniversityUserRoles()
        {
            var arms = new[]
            {
                new { Id = UniversityUserRoles.Leadership, Name = "القيادة" },
                new { Id = UniversityUserRoles.FieldActivists, Name = "نشطاء ميدانيين" },
                new { Id = UniversityUserRoles.StudentCouncilCommittees, Name = "لجان مجلس طلبة" }
            };

            return Ok(arms);
        }

        [HttpGet("/GetUserUniversityByRole")]
        public IActionResult GetUniversityUserRoles(int universityId, int universityStudentArmId, int roleId)
        {
            var userIds = _context.UserUniversityRole.Where(w =>
                w.UniversityId == universityId &&
                w.UniversityStudentArmId == universityStudentArmId &&
                w.RoleId == roleId).Select(s => s.UserId).ToList();

            var wantedUsers = _context.Users.Where(w => userIds.Contains(w.Id)).ToList();
            var modelToReturn = new List<UserModel>();

            foreach (var user in wantedUsers)
            {
                var usersModel = _userService.GetDetailedUserInformation(user);
                modelToReturn.Add(usersModel);
            }

            return Ok(modelToReturn);
        }

        [HttpGet("/GetUserUniversityRoleName")]
        public IActionResult GetUserUniversityRoleName(int universityId, int userId)
        {
            var roleName = _context.UserUniversityRole.FirstOrDefault(w =>
                w.UniversityId == universityId &&
                w.UserId == userId)?.Name;

            return Ok(roleName);
        }


        [HttpPost("AssignUserToStudentArm")]
        public IActionResult AssignUserToStudentArm([FromBody] AssignUserToStudentArmModel model)
        {
            try
            {
                if (model != null)
                {
                    // Validate if the user is already exist
                    if (!_context.Users.Any(u => u.Id == model.UserId))
                    {
                        return BadRequest("UserId is not exist.");
                    }

                    // Validate if the University is already exist
                    if (!_context.University.Any(u => u.Id == model.UniversityId))
                    {
                        return BadRequest("UniversityId is not exist.");
                    }

                    // Validate if the UniversityStudentArm is already exist
                    if (!_context.UniversityStudentArm.Any(u => u.Id == model.UniversityStudentArmId))
                    {
                        return BadRequest("UniversityStudentArm is not exist.");
                    }

                    if (_context.UserUniversityRole.Any(u => u.UniversityStudentArmId == model.UniversityStudentArmId &&
                        u.UniversityId == model.UniversityId &&
                        u.UserId == model.UserId))
                    {
                        return BadRequest("This User Role is already exist.");
                    }


                    List<int> roleIds = new List<int>((int[])Enum.GetValues(typeof(UniversityUserRoles)));

                    if (!roleIds.Contains(model.RoleId))
                    {
                        return BadRequest("RoleId is not exist.");
                    }

                    var row = new UserUniversityRoleEntity
                    {
                        Name = model.Name,
                        RoleId = model.RoleId,
                        UniversityId = model.UniversityId,
                        UserId = model.UserId,
                        UniversityStudentArmId = model.UniversityStudentArmId
                    };
                    var addedEntity = _context.UserUniversityRole.Add(row);
                    _context.SaveChanges();

                    return Ok(addedEntity.Entity);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
