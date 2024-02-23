using ApplicationData.Contract.Models.DataModels;

namespace ApplicationNotification.Notification
{
    public class ClockOutNotification
    {
        public EmployeeDataModel Employee { get; set; }
        public TimeClockDataModel TimeClock { get; set; }
    }
}
