using WebTimeClock.ApplicationData.Contract.Models.DataModels;

namespace WebTimeClock.ApplicationData.Contract.Services
{
    public interface IEmployeeDataService
    {
        public Task<EmployeeDataModel> GetEmployeeAsync(string employeeId);
    }
}
