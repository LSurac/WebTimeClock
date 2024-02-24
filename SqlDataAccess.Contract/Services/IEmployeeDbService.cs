using WebTimeClock.SqlDataAccess.Contract.Entities;

namespace WebTimeClock.SqlDataAccess.Contract.Services
{
    public interface IEmployeeDbService
    {
        public Task<Employee> GetEmployeeByIdAsync(string employeeId);
    }
}
