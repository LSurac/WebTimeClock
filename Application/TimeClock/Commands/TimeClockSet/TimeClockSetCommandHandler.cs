using Application.Models.Dto;
using ApplicationData.Contract.Models.DataModels;
using ApplicationData.Contract.Models.Enums;
using ApplicationData.Contract.Services;
using ApplicationNotification.Notification;
using MediatR;

namespace Application.TimeClock.Commands.TimeClockSet
{
    public class TimeClockSetCommandHandler(
        IEmployeeDataService employeeDataService,
        ITimeClockDataService timeClockDataService,
        IMediator mediator)
    {
        public async Task<TimeClockSetCommandResult> Handle(
            TimeClockSetCommand request,
            CancellationToken cancellationToken)
        {
            var employeeDataModel = await employeeDataService.GetEmployeeAsync(request.TimeClock.EmployeeId.ToString())
                ?? throw new Exception($"Invalid EmployeeId {request.TimeClock.EmployeeId}");

            var lastTimeClock =
                await timeClockDataService.GetLastTimeClockDataAsync(request.TimeClock.EmployeeId.ToString());

            TimeClockDataModel newTimeClock;

            if (lastTimeClock == null)
            {
                newTimeClock = new TimeClockDataModel
                {
                    Action = ETimeClockAction.CheckIn,
                    EmployeeId = request.TimeClock.EmployeeId,
                    UtcTime = DateTime.UtcNow
                };
            }
            else
            {
                newTimeClock = lastTimeClock;

                newTimeClock.Action = lastTimeClock.Action switch
                {
                    ETimeClockAction.CheckIn => ETimeClockAction.CheckOut,
                    ETimeClockAction.CheckOut => ETimeClockAction.CheckIn,
                    _ => newTimeClock.Action
                };
            }

            await timeClockDataService.SetTimeClockAsync(newTimeClock);

            var notification = new ClockOutNotification
            {
                TimeClock = newTimeClock,
                Employee = employeeDataModel
            };

            await mediator.Send(notification, cancellationToken);

            return new TimeClockSetCommandResult();
        }
    }
}
