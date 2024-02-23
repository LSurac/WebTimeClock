using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlDataAccess.Contract.Entities
{
    [Table("password")]
    public class Password
    {
        [Key][Column("password_Id")] public int Id { get; set; }
        [Column("password_ObjId")] public int EmployeeId { get; set; }
        [Column("password_Salt")] public string? Salt { get; set; }
        [Column("password_Hash")] public string? Hash { get; set; }
        [Column("password_IsActive")] public bool? IsActive { get; set; }
        [Column("password_FirstEditDate")] public DateTime? FirstEditDate { get; set; }
        [Column("password_LastEditDate")] public DateTime? LastEditDate { get; set; }
    }
}
