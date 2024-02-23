using MediatR;

namespace Application.TimeClock.Queries.TimeClockLastGet
{
    public class TimeClockLastGetQuery : IRequest<TimeClockLastGetQueryResult>
    {
        public string EmployeeId { get; set; }
    }
}
