using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebTimeClock.WebUi.Configuration;

namespace WebTimeClock.WebUi.Common
{
    public class TokenBuilderService(TokenSettings tokenSettings)
    {
        private const string ISSUER = "http://localhost:5000";
        private const string AUDIENCE = "http://localhost:5000";

        private JwtSecurityToken JwtSecurityTokenGet(
            IEnumerable<Claim> claimList,
            int expirationTimeInMinutes)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.TokenIssuerSigningKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                ISSUER,
                AUDIENCE,
                claimList,
                expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
                signingCredentials: signinCredentials
            );
        }

        public string AccessTokenBuild(
            List<Claim> claimList)
        {
            var jwtSecurityToken = JwtSecurityTokenGet(
                claimList,
                tokenSettings.AccessTokenExpirationTimeInMinutes);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public string AccessTokenBuildWithExpirationTime(
            List<Claim> claimList,
            int expirationTimeInMinutes)
        {
            var jwtSecurityToken = JwtSecurityTokenGet(
                claimList,
                expirationTimeInMinutes);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public string RefreshTokenBuild(
            List<Claim> claimList)
        {
            var jwtSecurityToken = JwtSecurityTokenGet(
                claimList,
                tokenSettings.RefreshTokenExpirationTimeInMinutes);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
