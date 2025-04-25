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

        [KingAttribute(KingAttributeEnum.UserRole.Admin)]
        public IActionResult ProtectedPage()
        {
            //NOTE: HttpContext.Items["CurrentUser"] is scoped per request, per user.
            //his HttpContext is fully isolated per user and per request. There’s no sharing between user
            var user = HttpContext.Items["CurrentUser"] as UserTokenDto;
            if (user == null) return RedirectToAction(TokenStatics.DestinationActionAfterLogout, TokenStatics.DestinationControllerAfterLogout);
            return View("ProtectedPage", user);
        }
    }
}