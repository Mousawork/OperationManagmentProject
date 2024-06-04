using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;

namespace OperationManagmentProject.Controllers
{
    public class WeaknessController : Controller
    {
        private readonly AppDbContext _context;

        public WeaknessController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("AddWeakness")]
        public IActionResult AddWeakness([FromBody] string name)
        {
            var row = new WeaknessType
            {
                Name = name
            };
            _context.WeaknessTypes.Add(row);
            _context.SaveChanges();

            return Ok("weakness type Added successful");
        }

    }
}
