using KingJwtAuth.Models;
using KingJwtAuth.Services.Token;
using KingJwtAuth.Services.UserLogs;
using KingJwtAuth.Services.UserRefreshToken;
using KingJwtAuth.Services;
using Microsoft.AspNetCore.Mvc;
using static KingJwtAuth.Attributes.KingAttributeEnum;
using System.Security.Claims;

namespace KingJwtAuth.Views.Shared.Components._KingBaseComponent
{
    public abstract class KingBaseViewComponent : ViewComponent
    {
        protected UserTokenDto _user;
        public KingBaseViewComponent()
        {
        }

        protected void CheckAuthorization(HttpContext context, params UserRole[] roles)
        {
            var serviceProvider = HttpContext.RequestServices;
            var userRefreshTokenService = serviceProvider.GetRequiredService<UserRefreshTokenService>();
            var userLogService = serviceProvider.GetRequiredService<UserLogsService>();
            var usersSuspiciousService = serviceProvider.GetRequiredService<UsersSuspiciousService>();

            _user = CheckAccessLogic(context, userRefreshTokenService, roles);
        }
        private UserTokenDto CheckAccessLogic(HttpContext httpContext, UserRefreshTokenService refreshTokenService, params UserRole[] roles)
        {
            // get the cookie
            var cookies = httpContext.Request.Cookies;
            cookies.TryGetValue(TokenStatics.AuthCookieName, out string? token);
            if (token == null)
            {
                // check refreshToken whether is valid or not based on expiration of AccessToken Cookie
                token = CheckRefreshCookie(httpContext, refreshTokenService);
                if (token == null) { return null; }
            }
            // validation process
            // NOTE: outoutUserTokenDto is a class that will be filled from cookie and we want to compare its decoded information to user cliams
            TokenAccessService tokenService = new TokenAccessService();
            var valid = tokenService.ValidateKingToken(token, TokenStatics.AccessTokenKey, out UserTokenDto outUserTokenDto);

            // check role
            try
            {
                if (valid && (roles.Any(r => r.ToString() == outUserTokenDto.Role)))
                {
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, outUserTokenDto.UserId),
                    new Claim(ClaimTypes.Role, outUserTokenDto.Role),
                };
                    var identity = new ClaimsIdentity(claims, "custom");
                    httpContext.User = new ClaimsPrincipal(identity);

                    return outUserTokenDto;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string CheckRefreshCookie(HttpContext httpContext, UserRefreshTokenService refreshTokenService)
        {
            // get the cookie
            var cookies = httpContext.Request.Cookies;
            cookies.TryGetValue(TokenStatics.RefreshCookieName, out string? outToken);
            if (outToken == null) return null;

            // get user' refreshToken from DB
            var encodedToken = EncryptionHelper.DecryptEncryptedGuid(outToken, TokenStatics.RefreshTokenKey);

            var userRefreshToken = refreshTokenService.GetRefreshTokenByToken(encodedToken);

            if (userRefreshToken == null) return null;
            if (encodedToken != userRefreshToken.Token) return null;


            // generate a new accessToken & refreshToken
            CookieService cookieService = new CookieService(httpContext);


            ///////////////////////////////////////////////////////////////            
            ///                      RefreshToken
            //NOTE: Be careful here, we don't need to create RefreshToken cookie anymore!
            ///////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////
            ///                      AccessToken
            ///////////////////////////////////////////////////////////////


            var exp = DateTimeOffset.UtcNow.AddDays(TokenStatics.ExpirationDayAuthCookie);// TODO: change [AddMinutes] to [AddDays]
            // generate token
            TokenAccessService tokenService = new TokenAccessService();
            UserTokenDto userTokenDto = new UserTokenDto()
            {
                UserId = userRefreshToken.UserId.ToString(),
                Role = userRefreshToken.Role,
                Exp = exp.UtcDateTime, // Convert DateTimeOffset to DateTime (UTC time)
            };
            string newAccessToken = tokenService.GenerateToken(userTokenDto, TokenStatics.AccessTokenKey);
            // generate cookie
            var checkCookie = cookieService.GenerateCookie(TokenStatics.AuthCookieName, newAccessToken, exp);

            ///////////////////////////////////////////////////////////////
            ///                      Returned Value
            ///////////////////////////////////////////////////////////////
            return newAccessToken;
        }

    }
}
