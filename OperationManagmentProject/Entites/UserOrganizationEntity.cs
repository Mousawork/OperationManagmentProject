﻿using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserOrganizationEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public int UserOrganizationRelationId { get; set; }      
    }
}

/*
 ALTER TABLE UserOrganization
ADD UserOrganizationRelationId INT;

 */