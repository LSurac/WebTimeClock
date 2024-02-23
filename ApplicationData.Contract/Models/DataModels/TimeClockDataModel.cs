using ApplicationData.Contract.Models.Enums;

namespace ApplicationData.Contract.Models.DataModels
{
    public class TimeClockDataModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime UtcTime { get; set; }
        public ETimeClockAction Action { get; set; }
    }
}
