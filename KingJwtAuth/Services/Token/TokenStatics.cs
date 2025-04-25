namespace KingJwtAuth.Services.Token
{
    public class TokenStatics
    {
        public static string AuthCookieName = "KingCookie";
        public static string RefreshCookieName = "KingRefreshCookie";

        //TODO: change the followign values to DECODE ones
        public static string AccessTokenKey = "your-secret-key-that-must-be-changed-to-a-good-one";
        public static string RefreshTokenKey = "your-different-secret-key-that-must-be-changed-to-a-good-one";

        //TODO: change the following values to ones are based on standard DAY EXPIRATION
        public static int ExpirationDayAuthCookie = 1;
        public static int ExpirationDayRefreshCookie = 2;

        public static string DestinationControllerAfterLogout = "Home";
        public static string DestinationActionAfterLogout = "Index";
    }
}
