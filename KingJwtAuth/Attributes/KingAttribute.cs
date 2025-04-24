using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using static KingJwtAuth.Attributes.KingAttributeEnum;
using KingJwtAuth.Services.Token;
using KingJwtAuth.Models;

namespace KingJwtAuth.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class KingAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserRole _role;
        public KingAttribute(UserRole role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            var user = CheckAccessLogic(httpContext, _role);
            if (user != null)
                RefreshCookie(httpContext, user); // Refresh the cookie if valid
            else
                context.Result = new RedirectToActionResult("Index", "Home", null); // unauthenticated user + redirect to main page or ...
        }
        private UserTokenDto CheckAccessLogic(HttpContext httpContext, UserRole role)
        {
            // get the cookie
            var cookies = httpContext.Request.Cookies;
            cookies.TryGetValue(TokenStatics.AuthCookieName, out string? token);
            if (token == null) return null;
            // validation process
            // NOTE: outoutUserTokenDto is a class that will be filled from cookie and we want to compare its decoded information to user cliams
            TokenService tokenService = new TokenService();
            var valid = tokenService.ValidateKingToken(token, TokenStatics.TokenKey, out UserTokenDto outoutUserTokenDto);
            // check role
            if (valid && (outoutUserTokenDto.Role == role.ToString())) return outoutUserTokenDto; else return null;
        }

        private void RefreshCookie(HttpContext httpContext, UserTokenDto userTokenDto)
        {
            // compute expiration dateTime
            //TODO: change [AddMinutes] to [AddDays]
            var exp = DateTimeOffset.UtcNow.AddMinutes(TokenStatics.ExpirationDayAuthCookie);
            // Generate a token
            //NOTE: when we are generating a TOKEN, we put the user's information (like ROLE) in it, not in Cookie directly!
            TokenService tokenService = new TokenService();
            var newToken = tokenService.GenerateKingToken(userTokenDto, TokenStatics.TokenKey);
            // reCreate the cookie
            CookieService cookieService = new CookieService(httpContext);
            cookieService.GenerateCookie(TokenStatics.AuthCookieName, newToken, exp);
        }
    }
}
