using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class UserTravel
    {
        [Key] // This attribute marks the property as the primary key
        public int Id { get; set; }

        public int UserId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}


/*
CREATE TABLE UserTravel (
    Id INT IDENTITY(1,1) NOT NULL,
    UserId INT NOT NULL,
    TravelDate DATETIME NOT NULL,
    ReturnDate DATETIME NOT NULL,
    Destination NVARCHAR(255) NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL,
    PRIMARY KEY (Id)
);
 */