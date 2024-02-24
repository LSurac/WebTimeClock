namespace WebTimeClock.WebUi.Configuration
{
    public class TokenSettings
    {
        public const string SectionName = "Token";

        public string? TokenIssuerSigningKey { get; set; }
        public int AccessTokenExpirationTimeInMinutes { get; set; }
        public int RefreshTokenExpirationTimeInMinutes { get; set; }
    }
}
