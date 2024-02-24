using WebTimeClock.ApplicationData.Contract.Models.DataModels;
using WebTimeClock.ApplicationData.Contract.Models.Enums;
using WebTimeClock.ApplicationData.Contract.Services;
using WebTimeClock.SqlDataAccess.Contract.Entities;
using WebTimeClock.SqlDataAccess.Contract.Services;

namespace WebTimeClock.ApplicationData.Services
{
    public class EmployeeDataService(IEmployeeDbService employeeDbService) : IEmployeeDataService
    {
        public async Task<EmployeeDataModel> GetEmployeeAsync(string employeeId)
        {
            var employeeEntity = await employeeDbService.GetEmployeeByIdAsync(employeeId);

            var employeeDataModel = MapEmployeeToDataModel(employeeEntity);

            return employeeDataModel;
        }

        private static EmployeeDataModel MapEmployeeToDataModel(Employee employeeEntity)
        {
            _ = Enum.TryParse<EGender>(employeeEntity.Gender, out var genderEnum);

            var employeeDataModel = new EmployeeDataModel
            {
                Id = employeeEntity.Id,
                ForName = employeeEntity.ForName,
                LastName = employeeEntity.LastName,
                AddressId = employeeEntity.AddressId.Value,
                DateOfBirth = employeeEntity.DateOfBirth.Value,
                DepartmentId = employeeEntity.DepartmentId.Value,
                Email = employeeEntity.Email,
                Gender = genderEnum,
                HireDate = employeeEntity.HireDate.Value,
                PhoneNumber = employeeEntity.PhoneNumber,
            };
            return employeeDataModel;
        }
    }
}
