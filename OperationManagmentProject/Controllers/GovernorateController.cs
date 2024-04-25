using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;

namespace OperationManagmentProject.Controllers
{
    public class GovernorateController : Controller
    {
        private readonly AppDbContext _context;

        public GovernorateController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/GetAllGovernorates")]
        public IActionResult GetGovernorates()
        {
            var governorates = _context.Governorate.Where(w=> w.ParentId == null || w.ParentId == 0).ToList();
            return Ok(governorates);
        }

        [HttpGet("/GetCitiesByGovernorateId/{id}")]
        public IActionResult GetCitiesByGovernorateId(int id)
        {
            var cities = _context.Governorate.Where(w => w.ParentId == id).ToList();
            return Ok(cities);
        }

        [HttpGet("/GetGovernorateById/{id}")]
        public IActionResult GetGovernorateById(int id)
        {
            var gov = _context.Governorate.FirstOrDefault(w => w.Id == id);
            return Ok(gov);
        }

        [HttpGet("/GetAllCities")]
        public IActionResult GetAllCities()
        {
            var cities2 = _context.Governorate.ToList();
            return Ok(cities2);
        }
    }
}
