using SqlDataAccess.Contract.Entities;

namespace SqlDataAccess.Contract.Services
{
    public interface IEmployeeDbService
    {
        public Task<Employee> GetEmployeeByIdAsync(string employeeId);
    }
}
