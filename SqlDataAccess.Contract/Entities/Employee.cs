using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTimeClock.SqlDataAccess.Contract.Entities
{
    [Table("employee")]
    public class Employee
    {
        [Key][Column("employee_Id")] public int Id { get; set; }
        [Column("employee_ForName")] public string? ForName { get; set; }
        [Column("employee_LastName")] public string? LastName { get; set; }
        [Column("employee_Gender")] public string? Gender { get; set; }
        [Column("employee_DateOfBirth")] public DateOnly? DateOfBirth { get; set; }
        [Column("employee_Email")] public string? Email { get; set; }
        [Column("employee_PhoneNumber")] public string? PhoneNumber { get; set; }
        [Column("employee_AddressId")] public int? AddressId { get; set; }
        [Column("employee_HireDate")] public DateOnly? HireDate { get; set; }
        [Column("employee_DepartmentId")] public int? DepartmentId { get; set; }
        [Column("employee_FirstEditDate")] public DateTime? FirstEditDate { get; set; }
        [Column("employee_LastEditDate")] public DateTime? LastEditDate { get; set; }
    }
}