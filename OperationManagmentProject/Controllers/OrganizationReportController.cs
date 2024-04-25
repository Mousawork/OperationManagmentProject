using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class OrganizationReportController : Controller
    {
        private readonly AppDbContext _context;

        public OrganizationReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateOrganizationReport")]
        public IActionResult CreateOrganizationReport([FromBody] CreateOrganizationReportModel model)
        {
            try
            {
                if (model != null)
                {
                    if (model.OrganizationId != 0)
                    {
                        if (!_context.Organization.Any(u => u.Id == model.OrganizationId))
                        {
                            return BadRequest("OrganizationId invalid.");
                        }

                        var newRow = new OrganizationReports
                        {
                            OrganizationId = model.OrganizationId,
                            Report = model.Report,
                            SessionNumber = model.SessionNumber,
                            InformationSource = model.InformationSource,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _context.OrganizationReports.Add(newRow);
                        _context.SaveChanges();

                        return Ok("Report created successful");
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

        [HttpGet("GetOrganizationReports")]
        public IActionResult GetOrganizationReports(int organizationId)
        {
            try
            {
                // Start with the base query
                var query = _context.OrganizationReports.AsQueryable();
                if (organizationId == 0)
                {
                    return BadRequest("Id is null");
                }

                query = query.Where(w => w.OrganizationId == organizationId);
                var result = query.ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("UpdateOrganizationReport")]
        public IActionResult UpdateOrganizationReport(int reportId, string? newReportContent)
        {
            try
            {
                // Retrieve the existing report entity from the database
                var report = _context.OrganizationReports.FirstOrDefault(w => w.Id == reportId);

                // Check if the report exists
                if (report == null)
                {
                    return NotFound("Report not found");
                }

                // Update the report content
                report.Report = newReportContent;

                // Save the changes to the database
                _context.SaveChanges();

                return Ok("Report updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteOrganizationReport")]
        public IActionResult DeleteUserReport(int reportId)
        {
            try
            {
                // Retrieve the report entity from the database
                var report = _context.OrganizationReports.FirstOrDefault(w => w.Id == reportId);

                // Check if the report exists
                if (report == null)
                {
                    return NotFound("Report not found");
                }

                // Remove the report from the database
                _context.OrganizationReports.Remove(report);

                // Save the changes to the database
                _context.SaveChanges();

                return Ok("Report deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
