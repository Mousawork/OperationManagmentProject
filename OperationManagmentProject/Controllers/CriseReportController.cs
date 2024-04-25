using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class CriseReportController : Controller
    {
        private readonly AppDbContext _context;

        public CriseReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateCriseReport")]
        public IActionResult CreateCriseReport([FromBody] CreateCriseReportModel model)
        {
            try
            {
                if (model != null)
                {
                    if (model.CriseId != 0)
                    {
                        if (!_context.Crises.Any(u => u.Id == model.CriseId))
                        {
                            return BadRequest("CriseId invalid.");
                        }

                        var newRow = new CriseReports
                        {
                            CriseId = model.CriseId,
                            Report = model.Report,
                            SessionNumber = model.SessionNumber,
                            InformationSource = model.InformationSource,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _context.CriseReports.Add(newRow);
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

        [HttpGet("GetCriseReports")]
        public IActionResult GetCriseReports(int criseId)
        {
            try
            {
                // Start with the base query
                var query = _context.CriseReports.AsQueryable();
                if (criseId == 0 || criseId == null)
                {
                    return BadRequest("Id is null");
                }

                query = query.Where(w => w.CriseId == criseId);
                var result = query.ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("UpdateCriseReport")]
        public IActionResult UpdateCriseReport(int reportId, string? newReportContent)
        {
            try
            {
                // Retrieve the existing report entity from the database
                var report = _context.CriseReports.FirstOrDefault(w => w.Id == reportId);

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

        [HttpDelete("DeleteCriseReport")]
        public IActionResult DeleteCriseReport(int reportId)
        {
            try
            {
                // Retrieve the report entity from the database
                var report = _context.CriseReports.FirstOrDefault(w => w.Id == reportId);

                // Check if the report exists
                if (report == null)
                {
                    return NotFound("Report not found");
                }

                // Remove the report from the database
                _context.CriseReports.Remove(report);

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
