using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class ActionEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
