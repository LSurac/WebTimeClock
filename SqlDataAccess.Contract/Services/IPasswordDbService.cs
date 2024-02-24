using WebTimeClock.SqlDataAccess.Contract.Entities;

namespace WebTimeClock.SqlDataAccess.Contract.Services
{
    public interface IPasswordDbService
    {
        public Task<List<Password>> GetPasswordListByEmployeeIdAsync(string employeeId);
    }
}
