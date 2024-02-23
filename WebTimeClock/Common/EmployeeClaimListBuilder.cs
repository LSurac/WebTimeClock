using System.Security.Claims;
using Application.Models.Dto;

namespace WebTimeClock.Common
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
