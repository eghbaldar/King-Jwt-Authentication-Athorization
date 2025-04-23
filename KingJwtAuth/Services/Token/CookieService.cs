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
        public string GenerateCookie(string cookieName, string token, DateTimeOffset exp)
        {
            _httpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // ok for local testing
                SameSite = SameSiteMode.Strict,
                Expires = exp
            });
            return $"Cookie '{cookieName}' set with expiration: {exp}";
        }
    }
}
