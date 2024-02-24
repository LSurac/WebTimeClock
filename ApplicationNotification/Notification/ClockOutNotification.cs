using ApplicationData.Contract.Models.DataModels;
using MediatR;

namespace ApplicationNotification.Notification
{
    public class ClockOutNotification : INotification
    {
        public EmployeeDataModel Employee { get; set; }
        public TimeClockDataModel TimeClock { get; set; }
    }
}
