using MediatR;

namespace Application.Employee.Queries.EmployeeGet
{
    public class EmployeeGetQuery : IRequest<EmployeeGetQueryResult>
    {
        public string EmployeeId { get; set; }
    }
}
