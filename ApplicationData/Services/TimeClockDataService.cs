using ApplicationData.Contract.Models.DataModels;
using ApplicationData.Contract.Models.Enums;
using ApplicationData.Contract.Services;
using SqlDataAccess.Contract.Entities;
using SqlDataAccess.Contract.Services;

namespace ApplicationData.Services
{
    public class TimeClockDataService(ITimeClockDbService timeClockDbService) : ITimeClockDataService
    {
        public async Task<List<TimeClockDataModel>> GetTimeClockDataListAsync(string employeeId, DateTime startDate, DateTime endDate)
        {
            var timeClockList = await timeClockDbService.GetTimeClockListByEmployeeIdAsync(employeeId, startDate, endDate);

            var timeClockDataModelList = new List<TimeClockDataModel>();

            foreach (var timeClock in timeClockList)
            {
                timeClockDataModelList.Add(await MapTimeClockToDataModel(timeClock));
            }

            return timeClockDataModelList;
        }

        public async Task<TimeClockDataModel> GetLastTimeClockDataAsync(string employeeId)
        {
            var timeClock = await timeClockDbService.GetLastTimeClockByEmployeeIdAsync(employeeId);

            return await MapTimeClockToDataModel(timeClock);
        }

        public async Task SetTimeClockAsync(TimeClockDataModel timeClockData)
        {
            var timeClock = await MapTimeClockDataModelToEntity(timeClockData);

            await timeClockDbService.SetTimeClockAsync(timeClock);
        }

        private static Task<TimeClockDataModel> MapTimeClockToDataModel(TimeClock timeClock)
        {
            _ = Enum.TryParse<ETimeClockAction>(timeClock.Action, out var timeClockActionEnum);

            return Task.FromResult(
                new TimeClockDataModel
                {
                    Id = timeClock.Id,
                    EmployeeId = timeClock.EmployeeId.Value,
                    Action = timeClockActionEnum,
                    UtcTime = timeClock.UtcTime.Value,
                });
        }

        private static Task<TimeClock> MapTimeClockDataModelToEntity(TimeClockDataModel timeClockDataModel)
        {
            return Task.FromResult(
                new TimeClock
                {
                    Id = timeClockDataModel.Id,
                    EmployeeId = timeClockDataModel.EmployeeId,
                    Action = timeClockDataModel.Action.ToString(),
                    UtcTime = timeClockDataModel.UtcTime,
                });
        }
    }
}
