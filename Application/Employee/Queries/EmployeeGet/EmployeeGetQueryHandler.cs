using Application.Employee.Queries.EmployeeGet;
using Application.Models.Dto;
using ApplicationData.Contract.Services;
using AutoMapper;
using MediatR;

namespace Application.Employee.Queries
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
