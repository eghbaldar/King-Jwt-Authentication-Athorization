namespace KingJwtAuth.Services.Token
{
    public class TokenStatics
    {
        public static string AuthCookieName = "KingCookie";
        public static string RefreshCookieName = "KingRefreshCookie";

        //TODO: change the followign values to DECODE ones
        //use:AES Encryption
        //for example use: https://anycript.com/crypto
        public static string AccessTokenKey = "your-secret-key-that-must-be-changed-to-a-good-one";
        public static string RefreshTokenKey = "your-different-secret-key-that-must-be-changed-to-a-good-one";

        //TODO: change the following values to ones are based on standard DAY EXPIRATION
        public static int ExpirationDayAuthCookie = 1;
        public static int ExpirationDayRefreshCookie = 3;

        public static string DestinationControllerAfterLogout = "Home";
        public static string DestinationActionAfterLogout = "Index";

        public static string BandPageController = "Auth";
        public static string BandPageAction = "Forbbiden";

        public static byte ExpirationHourBannedUser = 1; // it means the user cannot access to protected pages in [ExpirationHourBannedUser] hour.
        public static byte AllowedUserTriesToAccess = 10; // it means the user can ***frequently*** try only [AllowedUserTriesToAccess] times to access.
        public static sbyte AtLeastUserSuspiciousActivites = -10; // it means if the user tries [AllowedUserTriesToAccess] time less than [AtLeastUserSuspiciousActivites] the account is blocked

    }
}
