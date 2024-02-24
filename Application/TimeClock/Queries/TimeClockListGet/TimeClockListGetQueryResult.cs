using WebTimeClock.Application.Models.Dto;

namespace WebTimeClock.Application.TimeClock.Queries.TimeClockListGet
{
    public class TimeClockListGetQueryResult
    {
        public List<TimeClockDto> timeClockList {  get; set; } = new();
    }
}
