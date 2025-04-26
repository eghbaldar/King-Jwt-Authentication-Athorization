using KingJwtAuth.Attributes;
using KingJwtAuth.Models;
using KingJwtAuth.Services.Token;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KingJwtAuth.Controllers
{
    public class HomeController : Controller
    {
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
        [KingAttribute(KingAttributeEnum.UserRole.Admin, KingAttributeEnum.UserRole.User)]
        public IActionResult ProtectedPage2()
        {
            //NOTE: HttpContext.Items["CurrentUser"] is scoped per request, per user.
            //his HttpContext is fully isolated per user and per request. There’s no sharing between user
            var user = HttpContext.Items["CurrentUser"] as UserTokenDto;
            if (user == null) return RedirectToAction(TokenStatics.DestinationActionAfterLogout, TokenStatics.DestinationControllerAfterLogout);
            return View("ProtectedPage2", user);
        }
    }
}