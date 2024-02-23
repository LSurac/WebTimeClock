using ApplicationData.Contract.Models.DataModels;

namespace ApplicationData.Contract.Services
{
    public interface IEmployeeDataService
    {
        public Task<EmployeeDataModel> GetEmployeeAsync(string employeeId);
    }
}
