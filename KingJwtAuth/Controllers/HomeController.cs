using KingJwtAuth.Attributes;
using KingJwtAuth.Models;
using KingJwtAuth.Services.Token;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KingJwtAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public HomeController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateCookie()
        {
            // compute expiration dateTime
            //TODO: change [AddMinutes] to [AddDays]
            var exp = DateTimeOffset.UtcNow.AddMinutes(TokenStatics.ExpirationDayAuthCookie);
            // generate token
            TokenService tokenService = new TokenService();
            UserTokenDto userTokenDto = new UserTokenDto()
            {
                UserId = "e873ca06-d563-4c13-b3f5-5fa31a51d138",
                Role = "Admin",
                Exp = DateTime.Now.AddMinutes(TokenStatics.ExpirationDayAuthCookie)
            };
            string token = tokenService.GenerateKingToken(userTokenDto, TokenStatics.TokenKey);
            // generate cookie
            CookieService cookieService = new CookieService(_contextAccessor.HttpContext);
            var checkCookie = cookieService.GenerateCookie(TokenStatics.AuthCookieName, token, exp);

            return View("GenerateCookie", checkCookie);
        }
        [KingAttribute(KingAttributeEnum.UserRole.Admin)]
        public IActionResult ProtectedPage()
        {
            KingTokenAccessor _kingTokenAccessor = new KingTokenAccessor(_contextAccessor);
            UserTokenDto user = _kingTokenAccessor.GetUserFromToken();
            return View("ProtectedPage", user);
        }
    }
}
