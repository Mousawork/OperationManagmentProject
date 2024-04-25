namespace OperationManagmentProject.Entites
{
    using System;
    public class RelatedUsers
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PopulationUserId { get; set; }
        public string RelationType { get; set; }
        public string UserName { get; set; }
        public string Identity { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
