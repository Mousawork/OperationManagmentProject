using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class ReadCitiesController: Controller
    {
        private readonly AppDbContext _context;

        public ReadCitiesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("readJson")]
        public IActionResult ReadJson()
        {
            var directoryPath = @"C:\Projects\OperationManagmentProject\corrected.json";

            var json = System.IO.File.ReadAllText(directoryPath);
            var locations = JsonConvert.DeserializeObject<List<JsonLocationModel>>(json);


            foreach (var location in locations.DistinctBy(d => d.Governorate))
            {
                var newGovernorate = new Governorate
                {
                    Name = location.Governorate.Trim(),
                    Longitude = location.Longitude,
                    Latitude = location.Latitude
                };

                _context.Governorate.Add(newGovernorate);
                _context.SaveChanges();
            }

            var distinctGovernoratesGroups = locations.GroupBy(d => d.Governorate);

            foreach (var group in distinctGovernoratesGroups)
            {
                foreach (var governor in group)
                {
                    var governorateId = _context.Governorate.FirstOrDefault(f => f.Name == governor.Governorate)?.Id;

                    if (governorateId != null)
                    {
                        var newGovernorate = new Governorate
                        {
                            Name = governor.City,
                            ParentId = governorateId,
                            Longitude = governor.Longitude,
                            Latitude = governor.Latitude
                        };

                        _context.Governorate.Add(newGovernorate);
                        _context.SaveChanges();
                    }
                }

            }
  
            return Ok("Added successful");
        }


    }
}
