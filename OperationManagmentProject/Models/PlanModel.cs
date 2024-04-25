using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Models
{
    public class OverlayPlanModel
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
    }

    public class ItemPlanModel
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int IconId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Iconic { get; set; }
    }

    public class CoordinatesModel
    {
        public int Id { get; set; }
        public double MAltitude { get; set; }
        public double MLatitude { get; set; }
        public double MLongitude { get; set; }
    }

    public class PolygonDataModel
    {
        public int Id { get; set; }
        public required string Color { get; set; }
        public required List<CoordinatesModel> Coordinates { get; set; }
    }

    public class PlanModel
    {
        [Key]
        public int Id { get; set; }
        public int GovernorateId { get; set; }
        public List<ItemPlanModel>? ItemPlan { get; set; }
        public List<OverlayPlanModel>? OverlayPlan { get; set; }
        public required string Name { get; set; }
        public string ScreenShot { get; set; }
        public List<PolygonDataModel>? PolygonData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
