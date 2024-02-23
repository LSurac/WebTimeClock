using System.Data;
using System.Data.SqlClient;
using SqlDataAccess.Contract.Entities;
using SqlDataAccess.Contract.Services;

namespace SqlDataAccess.Services
{
    public class PasswordDbService(SqlDataAccessor sqlDataAccessor) : IPasswordDbService
    {
        public Task<List<Password>> GetPasswordListByEmployeeIdAsync(string employeeId)
        {
            var dbCommand = $"SELECT * FROM password WHERE password_employeeId = @employeeId";

            var parameters = new[]
            {
                new SqlParameter("@employeeId", SqlDbType.Int) { Value = 1 },
            };

            return sqlDataAccessor.GetEntityListAsync<Password>(dbCommand, parameters);
        }
    }
}
