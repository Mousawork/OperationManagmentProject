﻿using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class OrganizationReports
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? Report { get; set; }
        public string? SessionNumber { get; set; }
        public string? InformationSource { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
