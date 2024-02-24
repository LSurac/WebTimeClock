using System.Data;
using System.Data.SqlClient;
using WebTimeClock.SqlDataAccess.Contract.Entities;
using WebTimeClock.SqlDataAccess.Contract.Services;

namespace WebTimeClock.SqlDataAccess.Services
{
    public class TimeClockDbService(SqlDataAccessor sqlDataAccessor) : ITimeClockDbService
    {
        public Task<List<TimeClock>> GetTimeClockListByEmployeeIdAsync(string employeeId, DateTime startDate, DateTime endDate)
        {
            var dbCommand = $"SELECT * FROM timeclock WHERE timeclock_employeeId = @employeeId AND timeclock_UtcTime BETWEEN @startDate AND @endDate";

            var parameters = new[]
            {
                new SqlParameter("@employeeId", SqlDbType.Int) { Value = 1 },
                new SqlParameter("@startDate", SqlDbType.DateTime) { Value = startDate },
                new SqlParameter("@endDate", SqlDbType.DateTime) { Value = endDate }
            };

            return sqlDataAccessor.GetEntityListAsync<TimeClock>(dbCommand, parameters);
        }

        public Task<TimeClock> GetLastTimeClockByEmployeeIdAsync(string employeeId)
        {
            var dbCommand = $"SELECT TOP 1 * FROM timeclock WHERE timeclock_employeeId = {employeeId} ORDER BY timeclock_UtcTime DESC";

            var parameters = new[]
            {
                new SqlParameter("@employeeId", SqlDbType.Int) { Value = 1 },
            };

            return sqlDataAccessor.GetEntityAsync<TimeClock>(dbCommand, parameters);
        }

        public Task SetTimeClockAsync(TimeClock timeClock)
        {
            return sqlDataAccessor.SetEntityAsync(timeClock);
        }
    }
}
