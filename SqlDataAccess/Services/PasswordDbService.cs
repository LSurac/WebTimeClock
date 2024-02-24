using System.Data;
using System.Data.SqlClient;
using WebTimeClock.SqlDataAccess.Contract.Entities;
using WebTimeClock.SqlDataAccess.Contract.Services;

namespace WebTimeClock.SqlDataAccess.Services
{
    public class PasswordDbService(SqlDataAccessor sqlDataAccessor) : IPasswordDbService
    {
        public Task<List<Password>> GetPasswordListByEmployeeIdAsync(string employeeId)
        {
            var dbCommand = $"SELECT * FROM password WHERE password_ObjId = @passwordObjId";

            var parameters = new[]
            {
                new SqlParameter("@passwordObjId", SqlDbType.Int) { Value = 1 },
            };

            return sqlDataAccessor.GetEntityListAsync<Password>(dbCommand, parameters);
        }
    }
}
