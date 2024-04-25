using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites.MapProject
{
    public class PinnedMapObject
    {

        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }

        public string Value { get; set; }
    }
}
