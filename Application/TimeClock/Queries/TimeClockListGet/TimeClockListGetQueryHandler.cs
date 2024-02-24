using Application.Models.Dto;
using ApplicationData.Contract.Services;
using AutoMapper;
using MediatR;

namespace Application.TimeClock.Queries.TimeClockListGet
{
    public class TimeClockListGetQueryHandler(IEmployeeDataService employeeDataService,
        ITimeClockDataService timeClockDataService,
        IMapper mapper) : IRequestHandler<TimeClockListGetQuery, TimeClockListGetQueryResult>
    {
        public async Task<TimeClockListGetQueryResult> Handle(
            TimeClockListGetQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                throw new Exception("EmployeeId is missing");
            }

            _ = employeeDataService.GetEmployeeAsync(request.EmployeeId)
                ?? throw new Exception($"Invalid EmployeeId {request.EmployeeId}");

            var result = new TimeClockListGetQueryResult();

            var startDateUtc = request.StartDate.HasValue
                ? new DateTime(request.StartDate.Value.Year, request.StartDate.Value.Month, request.StartDate.Value.Day, 0, 0, 0, DateTimeKind.Utc)
                : DateTime.UtcNow;

            var endDateUtc = request.EndDate.HasValue
                ? new DateTime(request.EndDate.Value.Year, request.EndDate.Value.Month, request.EndDate.Value.Day, 23, 59, 59, DateTimeKind.Utc)
                : DateTime.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var timeClockDataModelList = await timeClockDataService.GetTimeClockDataListAsync(request.EmployeeId, startDateUtc, endDateUtc);

            foreach (var timeClockDto in timeClockDataModelList
                         .Select(timeClockDataModel => new { timeClockDataModel, timeClockDto = new TimeClockDto() })
                         .Select(t => mapper.Map<TimeClockDto>(t.timeClockDataModel)))
            {
                result.timeClockList.Add(timeClockDto);
            }

            return result;
        }
    }
}
