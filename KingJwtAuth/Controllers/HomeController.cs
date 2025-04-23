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
            // generate token
            TokenService tokenService = new TokenService();
            UserTokenDto userTokenDto = new UserTokenDto()
            {
                UserId = "e873ca06-d563-4c13-b3f5-5fa31a51d138",
                Role = "User",
                Exp = DateTime.Now.AddMinutes(3)
            };
            string token = tokenService.GenerateKingToken(userTokenDto, TokenStatics.TokenKey);
            // generate cookie
            CookieService cookieService = new CookieService(_contextAccessor.HttpContext);
            var checkCookie = cookieService.GenerateCookie(TokenStatics.AuthCookieName, token, TokenStatics.ExpirationAuthCookie);

            return View("GenerateCookie", checkCookie);
        }
        [KingAttribute(KingAttributeEnum.UserRole.Admin)]
        public IActionResult ProtectedPage()
        {
            return View();
        }
    }
}
