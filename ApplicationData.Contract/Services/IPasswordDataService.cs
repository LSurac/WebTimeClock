using ApplicationData.Contract.Models.DataModels;

namespace ApplicationData.Contract.Services
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
