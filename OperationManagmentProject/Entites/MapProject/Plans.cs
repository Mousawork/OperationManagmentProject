namespace OperationManagmentProject.Entites.MapProject
{
    using System.ComponentModel.DataAnnotations;

    public class Plans
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }
        public int GovernorateId { get; set; }
        public required string Name { get; set; }
        public string ScreenShot { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
