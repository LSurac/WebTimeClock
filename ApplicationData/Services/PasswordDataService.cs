using System.Security.Cryptography;
using System.Text;
using WebTimeClock.ApplicationData.Contract.Models.DataModels;
using WebTimeClock.ApplicationData.Contract.Services;
using WebTimeClock.SqlDataAccess.Contract.Entities;
using WebTimeClock.SqlDataAccess.Contract.Services;

namespace WebTimeClock.ApplicationData.Services
{
    public class PasswordDataService(IPasswordDbService passwordDbService) : IPasswordDataService
    {
        public async Task<PasswordDataModel> GetActivePasswordAsync(string employeeId)
        {
            try
            {
                var passwordList = await passwordDbService.GetPasswordListByEmployeeIdAsync(employeeId);

                var activePassword = passwordList.Find(p => p.IsActive.HasValue && p.IsActive.Value);

                if (activePassword == null)
                {
                    return null;
                }

                return await MapPasswordToDataModel(activePassword);
            }
            catch
            {
                return null;
            }
        }

        private static Task<PasswordDataModel> MapPasswordToDataModel(Password password)
        {
            return Task.FromResult(
                new PasswordDataModel
                {
                    Id = password.Id,
                    EmployeeId = password.EmployeeId,
                    IsActive = password.IsActive,
                    Hash = password.Hash,
                    Salt = password.Salt
                });
        }

        public string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(10));
        }

        public string ComputeHash(
            string password,
            string? salt)
        {
            var bytePassword = Encoding.UTF8.GetBytes(password);
            var byteSalt = Encoding.ASCII.GetBytes(salt);
            var hashAlgorithm = HashAlgorithmName.SHA256;
            var byteResult = Rfc2898DeriveBytes.Pbkdf2(bytePassword, byteSalt, 7, hashAlgorithm, 48);

            return Convert.ToBase64String(byteResult);
        }
    }
}
