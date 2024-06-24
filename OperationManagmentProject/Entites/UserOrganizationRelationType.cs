using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserOrganizationRelationType
    {
        [Key]
        public int Id { get; set; }
        public string? Type { get; set; }
    }
}


/*
 CREATE TABLE UserOrganizationRelationType (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Type NVARCHAR(50) NULL
);

-- Insert the specified values
INSERT INTO UserOrganizationRelationType (Type)
VALUES 
(N'المالك'),
(N'المدير العام'),
(N'مدير فرع'),
(N'موظف'),
(N'عميل');
 */