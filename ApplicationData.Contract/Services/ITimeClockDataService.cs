using WebTimeClock.ApplicationData.Contract.Models.DataModels;

namespace WebTimeClock.ApplicationData.Contract.Services
{
    public interface ITimeClockDataService
    {
        public Task<List<TimeClockDataModel>> GetTimeClockDataListAsync(string employeeId, DateTime startDate, DateTime endDate);
        public Task<TimeClockDataModel> GetLastTimeClockDataAsync(string employeeId);
        public Task SetTimeClockAsync(TimeClockDataModel timeClockData);
    }
}
