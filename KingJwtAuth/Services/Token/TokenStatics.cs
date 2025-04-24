namespace KingJwtAuth.Services.Token
{
    public class TokenStatics
    {
        public static string AuthCookieName = "KingCookie";
        public static string RefreshCookieName = "KingRefreshCookie";

        public static string TokenKey = "your-secret-key-that-must-be-changed-to-a-good-one";
        //TODO: change value to one is a standard based on Day
        public static int ExpirationDayAuthCookie = 1;
        public static int ExpirationDayRefreshCookie = 1;
        //public static DateTimeOffset ExpirationAuthCookie => DateTimeOffset.UtcNow.AddMinutes(10);
        //public static DateTimeOffset ExpirationRefreshCookie => DateTimeOffset.UtcNow.AddDays(30);
    }
}
