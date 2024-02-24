using MediatR;
using WebTimeClock.ApplicationData.Contract.Models.DataModels;
using WebTimeClock.ApplicationData.Contract.Models.Enums;
using WebTimeClock.ApplicationData.Contract.Services;
using WebTimeClock.ApplicationNotification.Notification;

namespace WebTimeClock.Application.TimeClock.Commands.TimeClockSet
{
    public class TimeClockSetCommandHandler(
        IEmployeeDataService employeeDataService,
        ITimeClockDataService timeClockDataService,
        IMediator mediator) : IRequestHandler<TimeClockSetCommand, TimeClockSetCommandResult>
    {
        public async Task<TimeClockSetCommandResult> Handle(
            TimeClockSetCommand request,
            CancellationToken cancellationToken)
        {
            var employeeDataModel = await employeeDataService.GetEmployeeAsync(request.TimeClock.EmployeeId.ToString())
                ?? throw new Exception($"Invalid EmployeeId {request.TimeClock.EmployeeId}");

            var lastTimeClock =
                await timeClockDataService.GetLastTimeClockDataAsync(request.TimeClock.EmployeeId.ToString());

            var newTimeClock = new TimeClockDataModel
            {
                EmployeeId = request.TimeClock.EmployeeId,
                UtcTime = DateTime.UtcNow,
            };

            newTimeClock.Action = lastTimeClock.Action switch
            {
                ETimeClockAction.CheckIn => ETimeClockAction.CheckOut,
                ETimeClockAction.CheckOut => ETimeClockAction.CheckIn,
                _ => lastTimeClock.Action
            };

            await timeClockDataService.SetTimeClockAsync(newTimeClock);

            var notification = new ClockOutNotification
            {
                TimeClock = newTimeClock,
                Employee = employeeDataModel
            };

            await mediator.Publish(notification, cancellationToken);

            return new TimeClockSetCommandResult();
        }
    }
}
