using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class OrganizationTypeController : Controller
    {
        private readonly AppDbContext _context;

        public OrganizationTypeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddOrganizationType")]
        public IActionResult AddOrganizationType([FromBody] OrganizationTypeEntity model)
        {
            try
            {
                if (model != null)
                {
                    // Validate if the Organization is already exist
                    if (_context.OrganizationType.Any(u => u.Name.ToLower() == model.Name.ToLower()))
                    {
                        return BadRequest("Organization Type already exist.");
                    }

                    var newOrganizationType = new OrganizationTypeEntity
                    {
                        Name = model.Name
                    };
                    var addedEntity = _context.OrganizationType.Add(newOrganizationType);
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

        [HttpGet("GetAllOrganizationTypes")]
        public IActionResult GetAllOrganizationTypes(string organizationTypeName)
        {
            try
            {
                // Start with the base query
                var query = _context.OrganizationType.AsQueryable();

                if (organizationTypeName != null)
                {
                    query = query.Where(w => w.Name.ToLower().Contains(organizationTypeName.ToLower()));
                }
                return Ok(query.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
