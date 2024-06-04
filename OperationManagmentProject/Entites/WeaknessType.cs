using System.ComponentModel.DataAnnotations;

namespace OperationManagmentProject.Entites
{
    public class WeaknessType
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}

/*
 
 CREATE TABLE WeaknessType (
    Id INT PRIMARY KEY,
    Name VARCHAR(50)
);

INSERT INTO WeaknessType (Id, Name, Translation) VALUES
(1, 'مال'),
(2, 'نساء'),
(3, 'سفر');
 */