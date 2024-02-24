using System.Security.Claims;
using WebTimeClock.Application.Models.Dto;

namespace WebTimeClock.WebUi.Common
{
    public class EmployeeClaimListBuilder
    {
        public List<Claim> EmployeeClaimListBuild(
            EmployeeDto employeeDto)
        {
            var claims = new List<Claim>
            {
                new Claim("EmployeeId", employeeDto.Id.ToString()),
            };

            return claims;
        }
    }
}
