namespace OperationManagmentProject.Entites
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Population
    {
        [Key] // This attribute marks the property as the primary key
        public int ID { get; set; }
        public string Identity { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string TName { get; set; }
        public string LName { get; set; }
        public string MName { get; set; }
        public string BirthDate { get; set; }
        public string Naheya { get; set; }
        public int GovernorateNo { get; set; }
        public string Governorate { get; set; }
        public string AreaNo { get; set; }
        public string Neighborhood { get; set; }
        public string CI_DEAD_DT { get; set; }
        public string NeighborhoodNo { get; set; }
        public string HouseNo { get; set; }
    }
}
