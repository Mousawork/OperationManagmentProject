using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly AppDbContext _context;

        public OrganizationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddOrganization")]
        public IActionResult AddOrganization([FromBody] AddOrganizationModel model)
        {
            try
            {
                if (model != null)
                {
                    // Validate if the type is already exist
                    if (!_context.OrganizationType.Any(u => u.Id == model.TypeId))
                    {
                        return BadRequest("Organization TypeId is not exist.");
                    }

                    // Validate if the type is already exist
                    if (!_context.PoliticalOrientationType.Any(u => u.Id == model.PoId))
                    {
                        return BadRequest("Organization PoId is not exist.");
                    }

                    // Validate if the Organization is already exist
                    if (_context.Organization.Any(u => u.Name.ToLower() == model.Name.ToLower()))
                    {
                        return BadRequest("Organization already exist.");
                    }

                    var newOrganization = new OrganizationEntity
                    {
                        Name = model.Name,
                        TypeId = model.TypeId,
                        PoId = model.PoId,
                        GovernorateId = model.GovernorateId,
                        Longitude = model.Longitude,
                        Latitude = model.Latitude,
                        Report = model.Report
                    };
                    var addedEntity = _context.Organization.Add(newOrganization);
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

        [HttpPut("UpdateOrganization")]
        public IActionResult UpdateOrganization(int organizationId, [FromBody] UpdateOrganizationModel model)
        {
            try
            {
                // Check if organizationId is valid
                if (organizationId <= 0)
                {
                    return BadRequest("Invalid organizationId.");
                }

                // Fetch the organization from the database
                var organization = _context.Organization.FirstOrDefault(o => o.Id == organizationId);
                if (organization == null)
                {
                    return BadRequest("Organization not found.");
                }

                // Validate if the type is already exist
                if (!_context.OrganizationType.Any(u => u.Id == model.TypeId))
                {
                    return BadRequest("Organization TypeId is not exist.");
                }

                // Validate if the type is already exist
                if (!_context.PoliticalOrientationType.Any(u => u.Id == model.PoId))
                {
                    return BadRequest("Organization PoId is not exist.");
                }

                // Validate if the Organization name already exists for another organization
                if (_context.Organization.Any(u => u.Id != organizationId && u.Name.ToLower() == model.Name.ToLower()))
                {
                    return BadRequest("Organization name already exists.");
                }

                // Update the organization properties
                organization.Name = model.Name;
                organization.TypeId = model.TypeId;
                organization.PoId = model.PoId;
                organization.GovernorateId = model.GovernorateId;
                organization.Longitude = model.Longitude;
                organization.Latitude = model.Latitude;
                organization.Report = model.Report;

                _context.SaveChanges();

                return Ok("Organization updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AssignUserToOrganization")]
        public IActionResult AssignUserToOrganization([FromBody] AssignUserToOrganizationModel model)
        {
            try
            {
                if (model != null)
                {
                    // Validate if the type is already exist
                    if (model.OrganizationId != null && model.UserId != null && model.OrganizationId != 0 && model.UserId != 0)
                    {
                        // Validate if the user is exist
                        if (!_context.Users.Any(u => u.Id == model.UserId))
                        {
                            return BadRequest("UserId invalid.");
                        }

                        // Validate if the Organization is already exist
                        if (!_context.Organization.Any(u => u.Id == model.OrganizationId))
                        {
                            return BadRequest("OrganizationId invalid.");
                        }

                        var newUserOrganization = new UserOrganizationEntity
                        {
                            UserId = model.UserId.Value,
                            OrganizationId = model.OrganizationId.Value
                        };
                        _context.UserOrganization.Add(newUserOrganization);
                        _context.SaveChanges();

                        return Ok("Organization Assigned successful");
                    }
                    return BadRequest();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteUserToOrganization")]
        public IActionResult DeleteUserToOrganization(int userId, int organizationId)
        {
            try
            {
                // Check if both userId and organizationId are valid
                if (userId <= 0 || organizationId <= 0)
                {
                    return BadRequest("Invalid userId or organizationId.");
                }

                // Check if the user is associated with the organization
                var userOrganization = _context.UserOrganization.FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == organizationId);
                if (userOrganization == null)
                {
                    return BadRequest("User is not associated with the organization.");
                }

                // Remove the association
                _context.UserOrganization.Remove(userOrganization);
                _context.SaveChanges();

                return Ok("User removed from organization successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllOrganizations")]
        public IActionResult GetAllOrganizations(string? organizationName, int? poId, int? governorateId)
        {
            try
            {
                // Start with the base query
                var query = _context.Organization.AsQueryable();

                if (organizationName != null)
                {
                    query = query.Where(w => w.Name.ToLower().Contains(organizationName.ToLower()));
                }
                if (poId != null && poId != 0)
                {
                    query = query.Where(w => w.PoId == poId);
                }
                if (governorateId != null && governorateId != 0)
                {
                    query = query.Where(w => w.GovernorateId == governorateId);
                }

                var result = query.Take(10).ToList();

                var modelToReturn = result.Select(s => new OrganizationModel
                {
                    Id = s.Id,
                    CreatedBy = s.CreatedBy,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Name = s.Name,
                    Report = s.Report,
                    PoId = s.PoId,
                    PoName = _context.PoliticalOrientationType.FirstOrDefault(w => w.Id == s.PoId)?.Type,
                    TypeId = s.TypeId,
                    TypeName = _context.OrganizationType.FirstOrDefault(w => w.Id == s.TypeId)?.Name
                });

                return Ok(modelToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetOrganizationInfoById")]
        public IActionResult GetOrganizationInfoById(int? organizationId)
        {
            try
            {
                if (organizationId != null && organizationId != 0)
                {
                    var org = _context.Organization.FirstOrDefault(w => w.Id == organizationId);
                    if (org == null) return BadRequest("Organization Id Invalid");
                    var relatedUsers = GetOrganizationRelatedUsers(organizationId);
                    var modelToReturn = new OrganizationModel
                    {
                        Id = org.Id,
                        CreatedBy = org.CreatedBy,
                        Latitude = org.Latitude,
                        Longitude = org.Longitude,
                        Name = org.Name,
                        Report = org.Report,
                        RelatedUser = relatedUsers,
                        PoId = org.PoId,
                        PoName = _context.PoliticalOrientationType.FirstOrDefault(w => w.Id == org.PoId)?.Type,
                        TypeId = org.TypeId,
                        TypeName = _context.OrganizationType.FirstOrDefault(w => w.Id == org.TypeId)?.Name
                    };
                    return Ok(modelToReturn);
                }

                return BadRequest("Organization Id Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private List<OrganizationRelatedUserModel> GetOrganizationRelatedUsers(int? organizationId)
        {
            var userIds = _context.UserOrganization.Where(w => w.OrganizationId == organizationId).Select(s => s.UserId);
            var list = new List<OrganizationRelatedUserModel>();
            foreach (var userId in userIds)
            {
                var userName = _context.Users.FirstOrDefault(f => f.Id == userId)?.FullName;
                if (userName == null) continue;
                list.Add(new OrganizationRelatedUserModel { Id = userId, Name = userName });
            }
            return list;
        }
    }
}
