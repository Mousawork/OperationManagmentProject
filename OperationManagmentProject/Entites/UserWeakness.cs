using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserWeakness
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }

        public int UserId { get; set; }
        public int WeaknessId { get; set; }
        public string? WeaknessName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}


/*
 CREATE TABLE UserWeakness (
    Id INT IDENTITY(1,1) NOT NULL,
    UserId INT NOT NULL,
    WeaknessId INT NOT NULL,
    WeaknessName NVARCHAR(255) NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    PRIMARY KEY (Id)
);
 */