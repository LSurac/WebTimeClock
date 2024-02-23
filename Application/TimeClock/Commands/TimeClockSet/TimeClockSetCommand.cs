using Application.Models.Dto;
using MediatR;

namespace Application.TimeClock.Commands.TimeClockSet
{
    public class TimeClockSetCommand : IRequest<TimeClockSetCommandResult>
    {
        public TimeClockDto TimeClock { get; set; }
    }
}
