using MediatR;

namespace WebTimeClock.Application.TimeClock.Queries.TimeClockListGet
{
    public class TimeClockListGetQuery : IRequest<TimeClockListGetQueryResult>
    {
        public string EmployeeId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
