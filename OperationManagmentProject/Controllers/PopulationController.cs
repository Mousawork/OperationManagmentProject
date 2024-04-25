using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class PopulationController : Controller
    {
        private readonly AppDbContext _context;

        public PopulationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddRelatedUser")]
        public IActionResult AddRelatedUser([FromBody] RelatedUsersModel data)
        {
            try
            {
                if (data == null)
                    return BadRequest();

                // Validate if the username is already taken
                if (!_context.Users.Any(u => u.Id == data.UserId))
                    return BadRequest("User is not exist.");

                if (!_context.Population.Any(u => u.ID == data.PopulationUserId))
                    return BadRequest("PopulationUserId is Invalid");

                var entity = new RelatedUsers
                {
                    UserId = data.UserId,
                    PopulationUserId = data.PopulationUserId,
                    RelationType = data.RelationType,
                    UserName = data.UserName,
                    Identity = data.Identity,
                    Longitude = data.Longitude,
                    Latitude = data.Latitude
                };

                var addedEntity = _context.RelatedUsers.Add(entity);
                _context.SaveChanges();
                return Ok(addedEntity?.Entity.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("DeleteRelatedUser")]
        public IActionResult DeleteRelatedUser([FromBody] DeleteRelatedUsersModel data)
        {
            try
            {
                var row = _context.RelatedUsers.FirstOrDefault(w => w.UserId == data.UserId && w.PopulationUserId == data.PopulationUserId);
                if (row == null)
                    return BadRequest("Relation not exist on the Database");

                _context.RelatedUsers.Remove(row);
                _context.SaveChanges();
                return Ok("relation deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPublicUserInfoById")]
        public IActionResult GetPublicUserInfoById(int? id)
        {
            try
            {
                if (id != null && id != 0)
                {
                    var users = _context.RelatedUsers.Where(w => w.UserId == id).ToList();
                    return Ok(users);
                }

                return BadRequest("Id Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/SearchPublicUserInfo")]
        public IActionResult SearchPublicUserInfo([FromQuery] PopulationModel filter)
        {
            try
            {
                var query = _context.Population
                        .Where(u =>
                            (string.IsNullOrEmpty(filter.FName) || u.FName.Contains(filter.FName)) &&
                            (string.IsNullOrEmpty(filter.SName) || u.SName.Contains(filter.SName)) &&
                            (string.IsNullOrEmpty(filter.TName) || u.TName.Contains(filter.TName)) &&
                            (string.IsNullOrEmpty(filter.LName) || u.LName.Contains(filter.LName)) &&
                            (string.IsNullOrEmpty(filter.Identity) || u.Identity == filter.Identity) &&
                            (string.IsNullOrEmpty(filter.Governorate) || u.Governorate == filter.Governorate) &&
                            (string.IsNullOrEmpty(filter.BirthDate) || u.BirthDate == filter.BirthDate)
                        );

                // Retrieve the filtered data
                var users = query.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
