using SqlDataAccess.Contract.Entities;

namespace SqlDataAccess.Contract.Services
{
    public interface IPasswordDbService
    {
        public Task<List<Password>> GetPasswordListByEmployeeIdAsync(string employeeId);
    }
}
