﻿namespace OperationManagmentProject.Models
{
    public class AssignUserToOrganizationModel
    {
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int UserOrganizationRelationId { get; set; }

    }
}
