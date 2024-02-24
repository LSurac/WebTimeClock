using WebTimeClock.SqlDataAccess.Contract.Entities;

namespace WebTimeClock.SqlDataAccess.Contract.Services
{
    public interface ITimeClockDbService
    {
        public Task<List<TimeClock>> GetTimeClockListByEmployeeIdAsync(string employeeId, DateTime startDate, DateTime endDate);
        public Task<TimeClock> GetLastTimeClockByEmployeeIdAsync(string employeeId);
        public Task SetTimeClockAsync(TimeClock timeClock);
    }
}
