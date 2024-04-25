using Microsoft.AspNetCore.Mvc;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class AddressController : Controller
    {
        private readonly AppDbContext _context;

        public AddressController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddAddress")]
        public IActionResult AddAddress([FromBody] AddAddressModel model)
        {
            try
            {
                if (model != null)
                {
                    // Validate if the username is already taken
                    if (!_context.Users.Any(u => u.Id == model.UserId))
                    {
                        return BadRequest("User not exist.");
                    }

                    var newAddress = new UserAddressEntity
                    {
                        UserId = model.UserId,
                        IsHome = model.IsHome,
                        IsWork = model.IsWork,
                        IsPopular = model.IsPopular,
                        Description = model.Description,
                        GovernorateId = model.GovernorateId,
                        CityId = model.CityId,
                        Longitude = model.Longitude,
                        Latitude = model.Latitude
                    };
                    var addedEntity = _context.UserAddresses.Add(newAddress);
                    _context.SaveChanges();

                    if (addedEntity != null)
                    {
                        if (model.AddressImages != null)
                        {
                            foreach (var image in model.AddressImages)
                            {
                                var newImage = new AddressImageEntity
                                {
                                    AddressId = addedEntity.Entity.AddressId,
                                    ImagePath = image.ImagePath
                                };
                                _context.AddressImages.Add(newImage);
                                _context.SaveChanges();
                            }
                        }
                    }

                    return Ok("Address Added successful");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
