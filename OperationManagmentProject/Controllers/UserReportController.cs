using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class UserReportController : Controller
    {
        private readonly AppDbContext _context;

        public UserReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateUserReport")]
        public IActionResult CreateUserReport([FromBody] CreateUserReportModel model)
        {
            try
            {
                if (model != null)
                {
                    if (model.UserId != 0)
                    {
                        // Validate if the user is exist
                        if (!_context.Users.Any(u => u.Id == model.UserId))
                        {
                            return BadRequest("UserId invalid.");
                        }

                        var newRow = new UserReports
                        {
                            UserId = model.UserId,
                            Report = model.Report,
                            SessionNumber = model.SessionNumber,
                            InformationSource = model.InformationSource,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _context.UserReports.Add(newRow);
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

        [HttpGet("GetUserReports")]
        public IActionResult GetUserReports(int userId)
        {
            try
            {
                // Start with the base query
                var query = _context.UserReports.AsQueryable();
                if (userId == 0)
                {
                    return BadRequest("Id is null");
                }

                query = query.Where(w => w.UserId == userId);
                var result = query.ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("UpdateUserReport")]
        public IActionResult UpdateUserReport(int reportId, string? newReportContent)
        {
            try
            {
                // Retrieve the existing report entity from the database
                var report = _context.UserReports.FirstOrDefault(w => w.Id == reportId);

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

        [HttpDelete("DeleteUserReport")]
        public IActionResult DeleteUserReport(int reportId)
        {
            try
            {
                // Retrieve the report entity from the database
                var report = _context.UserReports.FirstOrDefault(w => w.Id == reportId);

                // Check if the report exists
                if (report == null)
                {
                    return NotFound("Report not found");
                }

                // Remove the report from the database
                _context.UserReports.Remove(report);

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
