using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class ActionController : Controller
    {
        private readonly AppDbContext _context;

        public ActionController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet("GetUserActions")]
        public IActionResult GetUserActions(DateTime startDate, DateTime endDate)
        {
            var actions = _context.Action.ToList();
            var result = _context.UserActions.Where(w => w.CreatedAt >= startDate || w.CreatedAt <= endDate).Select(s => new
            {
                s.UserId,
                UserName = GetUserName(s.UserId),
                s.ActionId,
                ActionName = GetActionName(actions, s.ActionId),
                ActionDate = s.CreatedAt
            }).OrderByDescending(o=> o.ActionDate).ToList();

            _context.SaveChanges();

            return Ok("User Actions reterived successful");

        }

        private string? GetUserName(int userId)
        {
            return _context.Users.FirstOrDefault(f => f.Id == userId)?.FullName;
        }

        [HttpPost("AddUserAction")]
        public IActionResult AddUserAction([FromBody] AddUserActionModel model)
        {
            if (model != null)
            {
                // Validate if the username is already taken
                if (!_context.Users.Any(u => u.Id == model.UserId))
                {
                    return BadRequest("User not exist.");
                }

                // Validate if the username is already taken
                if (!_context.Action.Any(u => u.Id == model.ActionId))
                {
                    return BadRequest("ActionId not exist.");
                }

                var newUserAction = new UserActionEntity
                {
                    UserId = model.UserId,
                    ActionId = model.ActionId,
                    CreatedAt = DateTime.UtcNow,
                    Report = model.Report
                };
                _context.UserActions.Add(newUserAction);
                _context.SaveChanges();

                return Ok("User Action Added successful");
            }

            return BadRequest();
        }

        [HttpPost("AddOrganizationAction")]
        public IActionResult AddOrganizationAction([FromBody] AddOrganizationActionModel model)
        {
            if (model != null)
            {
                // Validate if the username is already taken
                if (!_context.Organization.Any(u => u.Id == model.OrganizationId))
                {
                    return BadRequest("OrganizationId not exist.");
                }

                // Validate if the username is already taken
                if (!_context.Action.Any(u => u.Id == model.ActionId))
                {
                    return BadRequest("ActionId not exist.");
                }

                var newOrgAction = new OrganizationAction
                {
                    OrganizationId = model.OrganizationId,
                    ActionId = model.ActionId,
                    Report = model.Report,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow

                };
                _context.OrganizationAction.Add(newOrgAction);
                _context.SaveChanges();

                return Ok("Organization Action Added successful");
            }

            return BadRequest();
        }

        [HttpPost("AddCrisesAction")]
        public IActionResult AddCrisesAction([FromBody] AddCriseActionModel model)
        {
            if (model != null)
            {
                // Validate if the username is already taken
                if (!_context.Crises.Any(u => u.Id == model.CriseId))
                {
                    return BadRequest("CriseId not exist.");
                }

                // Validate if the username is already taken
                if (!_context.Action.Any(u => u.Id == model.ActionId))
                {
                    return BadRequest("ActionId not exist.");
                }

                var newAction = new CriseActionEntity
                {
                    CriseId = model.CriseId,
                    ActionId = model.ActionId,
                    Report = model.Report,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow

                };
                _context.CrisesActions.Add(newAction);
                _context.SaveChanges();

                return Ok("Crise Action Added successful");
            }

            return BadRequest();
        }


        [HttpGet("GetOrganizationActions")]
        public IActionResult GetOrganizationActions(int organizationId)
        {
            // Validate if the username is already taken
            if (!_context.Organization.Any(u => u.Id == organizationId))
            {
                return BadRequest("OrganizationId not exist.");
            }

            var actions = _context.Action.ToList();
            var result = _context.OrganizationAction.Where(w => w.OrganizationId == organizationId).Select(s => new
            {
                s.Id,
                s.ActionId,
                s.OrganizationId,
                ActionName = GetActionName(actions, s.ActionId),
                s.Report,
                s.CreatedAt
            });

            return Ok(result);
        }


        [HttpPatch("UpdateCriseActionReport")]
        public IActionResult UpdateCriseActionReport([FromBody] UpdateCriseActionModel model)
        {

            var criseAction = _context.CrisesActions.FirstOrDefault(f => f.Id == model.Id);

            if (criseAction == null)
            {
                return BadRequest("not existing Id.");
            }

            criseAction.Report = model.Report;
            _context.SaveChanges();

            return Ok("Crise Action Updated successful");
        }

        [HttpPatch("UpdateUserActionReport")]
        public IActionResult UpdateUserActionReport([FromBody] UpdateUserActionModel model)
        {

            var userAction = _context.UserActions.FirstOrDefault(f => f.Id == model.Id);

            if (userAction == null)
            {
                return BadRequest("not existing Id.");
            }

            userAction.Report = model.Report;
            _context.SaveChanges();

            return Ok("User Action Updated successful");
        }

        [HttpPatch("UpdateOrganizationActionReport")]
        public IActionResult UpdateOrganizationActionReport([FromBody] UpdateOrganizationActionModel model)
        {

            var orgAction = _context.OrganizationAction.FirstOrDefault(f => f.Id == model.Id);

            if (orgAction == null)
            {
                return BadRequest("not existing Id.");
            }

            orgAction.Report = model.Report;
            _context.SaveChanges();

            return Ok("User Action Updated successful");
        }

        [HttpPost("AddAction")]
        public IActionResult AddAction([FromBody] AddActionModel model)
        {
            if (model != null)
            {
                // Validate if the username is already taken
                if (_context.Action.Any(u => u.Name == model.Action))
                {
                    return BadRequest("Action already exist.");
                }

                var newAction = new ActionEntity
                {
                    Name = model.Action
                };
                _context.Action.Add(newAction);
                _context.SaveChanges();

                return Ok("Action Added successful");
            }

            return BadRequest();
        }

        [HttpGet("GetActions")]
        public IActionResult GetActions()
        {
            return Ok(_context.Action.ToList());
        }

        [HttpGet("/GetActionById/{id}")]
        public IActionResult GetActionById(int id)
        {
            var action = _context.Action.FirstOrDefault(w => w.Id == id);
            return Ok(action);
        }

        static string? GetActionName(List<ActionEntity> actions, int actionId)
        {
            return actions.FirstOrDefault(w => w.Id == actionId)?.Name;
        }
    }
}
