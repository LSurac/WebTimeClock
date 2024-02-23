using Application.Models.Dto;
using ApplicationData.Contract.Models.DataModels;
using ApplicationData.Contract.Services;
using AutoMapper;

namespace Application.Employee.Commands.EmployeeValidateLogin
{
    public class EmployeeValidateLoginCommandHandler(IMapper mapper, 
        IEmployeeDataService employeeDataService, 
        IPasswordDataService passwordDataService)
    {
        public async Task<EmployeeValidateLoginCommandResult> Handle(
            EmployeeValidateLoginCommand request,
            CancellationToken cancellationToken)
        {
            EmployeeDataModel? employeeDataModel = null;

            if (!string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                employeeDataModel = await employeeDataService.GetEmployeeAsync(request.EmployeeId);
            }

            if (employeeDataModel == null)
            {
                throw new Exception("Invalid Login Data");
            }

            var passwordData = await passwordDataService.GetActivePasswordAsync(request.EmployeeId);

            var inputPassword = passwordDataService.ComputeHash(request.Password, passwordData.Salt);

            if (string.CompareOrdinal(passwordData.Hash, inputPassword) != 0)
            {
                throw new Exception("Invalid Login Data");
            }

            return new EmployeeValidateLoginCommandResult
            {
                Employee = mapper.Map<EmployeeDto>(employeeDataModel)
            };
        }
    }
}
