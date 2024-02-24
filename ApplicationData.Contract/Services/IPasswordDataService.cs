using WebTimeClock.ApplicationData.Contract.Models.DataModels;

namespace WebTimeClock.ApplicationData.Contract.Services
{
    public interface IPasswordDataService
    {
        public Task<PasswordDataModel> GetActivePasswordAsync(string employeeId);
        public string GenerateSalt();

        public string ComputeHash(
            string password,
            string? salt);
    }
}
