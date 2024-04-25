using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;

namespace OperationManagmentProject.Controllers
{
    public class PoliticalOrientationController : Controller
    {
        private readonly AppDbContext _context;

        public PoliticalOrientationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/GetAllPoliticalOrientations")]
        public IActionResult GetGovernorates()
        {
            var politicalOrientations = _context.PoliticalOrientationType.ToList();
            return Ok(politicalOrientations);
        }
    }
}
