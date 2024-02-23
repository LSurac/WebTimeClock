using Application.Models.Dto;

namespace Application.TimeClock.Queries.TimeClockListGet
{
    public class TimeClockListGetQueryResult
    {
        public List<TimeClockDto> timeClockList {  get; set; } = new();
    }
}
