using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace KingJwtAuth.Services.Token
{
    public class CookieService
    {
        private readonly HttpContext _httpContext;
        public CookieService(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public string GenerateCookie(string cookieName, string value, DateTimeOffset exp)
            {
            //TODO: remove the following code
            var test = DateTimeOffset.UtcNow;

            _httpContext.Response.Cookies.Append(cookieName, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // NOTE: change this value to TRUE for development environment!
                SameSite = SameSiteMode.Lax,
                //Expires = exp
                Expires = exp
            });
            return $"Cookie '{cookieName}' set with expiration: {exp}";
        }
    }
}
