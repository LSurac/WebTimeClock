using MediatR;

namespace WebTimeClock.Application.Employee.Commands.EmployeeValidateLogin
{
    public class EmployeeValidateLoginCommand : IRequest<EmployeeValidateLoginCommandResult>
    {
        public string EmployeeId { get; set; }
        public string Password { get; set; }
    }
}
