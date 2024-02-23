using System.Data;
using System.Data.SqlClient;
using SqlDataAccess.Contract.Entities;
using SqlDataAccess.Contract.Services;

namespace SqlDataAccess.Services
{
    public class EmployeeDbService(SqlDataAccessor sqlDataAccessor) : IEmployeeDbService
    {
        public Task<Employee> GetEmployeeByIdAsync(string employeeId)
        {
            var dbCommand = $"SELECT * FROM employee WHERE employee_Id = @employeeId";

            var parameters = new[]
            {
                new SqlParameter("@employeeId", SqlDbType.Int) { Value = 1 },
            };

            return sqlDataAccessor.GetEntityAsync<Employee>(dbCommand, parameters);
        }
    }
}
