using MediatR;
using WebTimeClock.Application.Models.Dto;

namespace WebTimeClock.Application.TimeClock.Commands.TimeClockSet
{
    public class TimeClockSetCommand : IRequest<TimeClockSetCommandResult>
    {
        public TimeClockDto TimeClock { get; set; }
    }
}
