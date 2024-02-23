using Application.Employee.Commands.EmployeeValidateLogin;
using Application.Employee.Queries.EmployeeGet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTimeClock.Common;
using WebTimeClock.Configuration;

namespace WebTimeClock.Controllers
{
    [ApiController]
    public class EmployeeController(TokenBuilderService tokenBuilderService,
            EmployeeClaimListBuilder claimListBuilder,
            TokenSettings tokenSettings)
        : BaseController<EmployeeController>
    {
        private const string REFRESH_TOKEN_COOKIE_NAME = "RefreshToken";

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(EmployeeGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> EmployeeGet(EmployeeGetQuery employeeGetQuery)
        {
            return Ok(
                await Mediator.Send(
                    employeeGetQuery
                )
            );
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> EmployeeLogin([FromBody] EmployeeValidateLoginCommand employeeLoginCommand)
        {
            var userValidateLoginCommandResult = await Mediator.Send(employeeLoginCommand);
            var accessTokenClaimList = claimListBuilder.EmployeeClaimListBuild(userValidateLoginCommandResult.Employee);
            var accessToken = tokenBuilderService.AccessTokenBuild(accessTokenClaimList);
            var refreshTokenClaimList = claimListBuilder.EmployeeClaimListBuild(userValidateLoginCommandResult.Employee);
            var refreshToken = tokenBuilderService.RefreshTokenBuild(refreshTokenClaimList);

            Response.Cookies.Append(
                REFRESH_TOKEN_COOKIE_NAME,
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(tokenSettings.RefreshTokenExpirationTimeInMinutes),
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

            return Ok(accessToken);
        }
    }
}
