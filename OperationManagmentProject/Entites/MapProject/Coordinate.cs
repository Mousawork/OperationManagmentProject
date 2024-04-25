using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class Coordinate
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public required int PolygonDataId { get; set; }
        public double MAltitude { get; set; }
        public double MLatitude { get; set; }
        public double MLongitude { get; set; }
    }
}
