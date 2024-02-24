using AutoMapper;
using MediatR;
using WebTimeClock.Application.Employee.Queries.EmployeeGet;
using WebTimeClock.Application.Models.Dto;
using WebTimeClock.ApplicationData.Contract.Services;

namespace WebTimeClock.Application.Employee.Queries
{
    public class EmployeeGetQueryHandler(IEmployeeDataService employeeDataService,
        IMapper mapper) : IRequestHandler<EmployeeGetQuery, EmployeeGetQueryResult>
    {
        public async Task<EmployeeGetQueryResult> Handle(
            EmployeeGetQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                throw new Exception("employeeId is missing");
            }

            var result = new EmployeeGetQueryResult();
            var employeeDataModel = await employeeDataService.GetEmployeeAsync(request.EmployeeId) 
                                    ?? throw new Exception($"couldn't find employee with Id = {request.EmployeeId}");

            result.Employee = mapper.Map<EmployeeDto>(employeeDataModel);

            return result;
        }
    }
}
