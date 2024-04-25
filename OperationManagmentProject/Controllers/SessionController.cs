using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;

namespace OperationManagmentProject.Controllers
{
    public class SessionController : Controller
    {
        private readonly AppDbContext _context;

        public SessionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddSession")]
        public IActionResult AddSession()
        {
            var addedRow = _context.Session.Add(new Session
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
            _context.SaveChanges();

            return Ok(addedRow?.Entity.Id);
        }


        [HttpGet("/GetSessionById/{id}")]
        public IActionResult GetSessionById(int id)
        {
            var session = _context.Session.FirstOrDefault(w => w.Id == id);
            return Ok(session);
        }
    }
}
