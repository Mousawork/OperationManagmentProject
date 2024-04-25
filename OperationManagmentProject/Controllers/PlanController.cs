using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OperationManagmentProject.Data;
using OperationManagmentProject.Entites.MapProject;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Controllers
{
    public class PlanController : Controller
    {
        private readonly AppDbContext _context;

        public PlanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("InsertPlan")]
        public IActionResult InsertPlan([FromBody] PlanModel data)
        {
            try
            {
                if (data == null)
                    return BadRequest();

                if (data.Name.IsNullOrEmpty())
                    return BadRequest("Name is required");

                var planEntity = new Plans
                {
                    GovernorateId = data.GovernorateId,
                    Name = data.Name,
                    ScreenShot = data.ScreenShot
                };

                var addedEntity = _context.Plans.Add(planEntity);
                _context.SaveChanges();

                if (data.ItemPlan != null && data.ItemPlan.Any())
                {
                    CreateItemPlans(data.ItemPlan, addedEntity.Entity.Id);
                }

                if (data.OverlayPlan != null && data.OverlayPlan.Any())
                {
                    CreateOverlayPlan(data.OverlayPlan, addedEntity.Entity.Id);
                }

                if (data.PolygonData != null && data.PolygonData.Any())
                {
                    CreatePolygonData(data.PolygonData, addedEntity.Entity.Id);
                }
                return Ok(addedEntity?.Entity.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPlansByGovernorateId")]
        public IActionResult GetPlansByGovernorateId(int? governorateId)
        {
            try
            {
                if (governorateId != null && governorateId != 0)
                {
                    var plans = _context.Plans.Where(w => w.GovernorateId == governorateId).ToList();
                    var modelToReturn = new List<PlanModel>();
                    foreach (var plan in plans)
                    {
                        modelToReturn.Add(new PlanModel
                        {
                            Id = plan.Id,
                            Name = plan.Name,
                            GovernorateId = plan.GovernorateId,
                            ItemPlan = GetItemsPlan(plan.Id),
                            OverlayPlan = GetOverlayPlan(plan.Id),
                            PolygonData = GetPolygonData(plan.Id),
                            ScreenShot = plan.ScreenShot,
                            CreatedAt = plan.CreatedAt
                        });
                    };
                    return Ok(modelToReturn);
                }

                return BadRequest("Governorate Id Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeletePlanById/{id}")]
        public IActionResult DeletePlanById(int id)
        {
            try
            {
                // Check if the plan with the given ID exists
                var planToDelete = _context.Plans.Find(id);
                if (planToDelete == null)
                {
                    return NotFound("Plan not found");
                }

                _context.Plans.Remove(planToDelete);
                _context.SaveChanges();

                return Ok("Plan deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetPinnedMapObject")]
        public IActionResult GetPinnedMapObject()
        {
            try
            {
                var pinnedMapObjects = _context.PinnedMapObject.ToList();
                return Ok(pinnedMapObjects);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("InsertPlanPoint")]
        public IActionResult InsertPlanPoint([FromBody] PlanPoints data)
        {
            try
            {
                if (data == null)
                    return BadRequest();

                var entity = new PlanPoints
                {
                    GovernorateId = data.GovernorateId,
                    Description = data.Description,
                    Latitude = data.Latitude,
                    Longitude = data.Longitude
                };

                var addedEntity = _context.PlanPoints.Add(entity);
                _context.SaveChanges();
                return Ok(addedEntity?.Entity.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPlanPointsByGovernorateId")]
        public IActionResult GetPlanPointsByGovernorateId(int? governorateId)
        {
            try
            {
                if (governorateId != null && governorateId != 0)
                {
                    var points = _context.PlanPoints.Where(w => w.GovernorateId == governorateId).ToList();
                    return Ok(points);
                }

                return BadRequest("Governorate Id Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        private List<PolygonDataModel> GetPolygonData(int planId)
        {
            var polygons = _context.PolygonData.Where(w => w.PlanId == planId).ToList();
            var modelToReturn = new List<PolygonDataModel>();

            foreach (var polygon in polygons)
            {
                modelToReturn.Add(new PolygonDataModel
                {
                    Id = polygon.Id,
                    Color = polygon.Color,
                    Coordinates = GetPolygonCoordinates(polygon.Id)
                });
            };

            return modelToReturn;
        }
        private List<CoordinatesModel> GetPolygonCoordinates(int polygonId)
        {
            var coordinates = _context.Coordinate.Where(w => w.PolygonDataId == polygonId).ToList();
            var modelToReturn = new List<CoordinatesModel>();

            foreach (var coordinate in coordinates)
            {
                modelToReturn.Add(new CoordinatesModel
                {
                    Id = coordinate.Id,
                    MAltitude = coordinate.MAltitude,
                    MLatitude = coordinate.MLatitude,
                    MLongitude = coordinate.MLongitude
                });
            };

            return modelToReturn;
        }
        private List<ItemPlanModel> GetItemsPlan(int planId)
        {
            var items = _context.ItemPlan.Where(w => w.PlanId == planId).ToList();
            var modelToReturn = new List<ItemPlanModel>();

            foreach (var item in items)
            {
                modelToReturn.Add(new ItemPlanModel
                {
                    Id = item.Id,
                    IconId = item.IconId,
                    PlanId = item.PlanId,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Iconic = item.Iconic
                });
            };

            return modelToReturn;
        }
        private List<OverlayPlanModel> GetOverlayPlan(int planId)
        {
            var items = _context.OverlayPlan.Where(w => w.PlanId == planId).ToList();
            var modelToReturn = new List<OverlayPlanModel>();

            foreach (var item in items)
            {
                modelToReturn.Add(new OverlayPlanModel
                {
                    Id = item.Id,
                    PlanId = item.PlanId,
                    StartLatitude = item.StartLatitude,
                    StartLongitude = item.StartLongitude,
                    EndLatitude = item.EndLatitude,
                    EndLongitude = item.EndLongitude,
                });
            };

            return modelToReturn;
        }

        private void CreateItemPlans(List<ItemPlanModel> itemPlan, int planId)
        {
            foreach (var itemPlanItem in itemPlan)
            {
                var itemEntity = new ItemPlan
                {
                    PlanId = planId,
                    IconId = itemPlanItem.IconId,
                    Latitude = itemPlanItem.Latitude,
                    Longitude = itemPlanItem.Longitude,
                    Iconic = itemPlanItem.Iconic
                };

                _context.ItemPlan.Add(itemEntity);
                _context.SaveChanges();
            }
        }

        private void CreateOverlayPlan(List<OverlayPlanModel> overlayPlanItems, int planId)
        {
            foreach (var overlayPlanItem in overlayPlanItems)
            {
                var entity = new OverlayPlan
                {
                    PlanId = planId,
                    StartLatitude = overlayPlanItem.StartLatitude,
                    StartLongitude = overlayPlanItem.StartLongitude,
                    EndLatitude = overlayPlanItem.EndLatitude,
                    EndLongitude = overlayPlanItem.EndLongitude
                };

                _context.OverlayPlan.Add(entity);
                _context.SaveChanges();
            }
        }

        private void CreatePolygonData(List<PolygonDataModel> polygonData, int planId)
        {
            foreach (var polygonItem in polygonData)
            {
                var polygonEntity = new PolygonData
                {
                    PlanId = planId,
                    Color = polygonItem.Color
                };

                var addedEntity = _context.PolygonData.Add(polygonEntity);
                _context.SaveChanges();


                if (addedEntity != null && polygonItem.Coordinates.Any())
                {
                    CreatePolygonCoordinates(polygonItem.Coordinates, addedEntity.Entity.Id);
                }

            }
        }
        private void CreatePolygonCoordinates(List<CoordinatesModel> coordinates, int PolygonId)
        {
            try
            {
                foreach (var item in coordinates)
                {
                    var entity = new Coordinate
                    {
                        PolygonDataId = PolygonId,
                        MAltitude = item.MAltitude,
                        MLatitude = item.MLatitude,
                        MLongitude = item.MLongitude
                    };

                    _context.Coordinate.Add(entity);
                    _context.SaveChanges();
                }
            }
            catch
            {
                return;
            }

        }
    }
}
