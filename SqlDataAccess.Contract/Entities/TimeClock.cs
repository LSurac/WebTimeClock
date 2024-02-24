using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTimeClock.SqlDataAccess.Contract.Entities
{
    [Table("timeclock")]
    public class TimeClock
    {
        [Key][Column("timeclock_Id")] public int Id { get; set; }
        [Column("timeclock_EmployeeId")] public int? EmployeeId { get; set; }
        [Column("timeclock_UtcTime")] public DateTime? UtcTime { get; set; }
        [Column("timeclock_Action")] public string? Action { get; set; }
    }
}