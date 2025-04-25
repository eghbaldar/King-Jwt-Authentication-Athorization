using KingJwtAuth.Context;
using KingJwtAuth.Models;
using KingJwtAuth.Services.Token;
using KingJwtAuth.Services.UserRefreshToken;
using KingJwtAuth.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KingJwtAuth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsersService _usersService;
        private readonly IDataBaseContext _context;
        public AuthController(
            IUsersService usersService,
            IHttpContextAccessor contextAccessor,
            IDataBaseContext context)
        {
            _usersService = usersService;
            _contextAccessor = contextAccessor;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin req)
        {
            // check user!
            var user = _usersService.GetUserById(new RequestGetUsersServiceDto
            {
                Username = req.Username,
                Password = req.Password,
            });
            if (user == null) return Json(new { IsSuccess = false });

            ///////////////////////////////////////////////////////////////
            ///                      RefreshToken
            ///////////////////////////////////////////////////////////////


            // generate the refresh cookie
            CookieService cookieService = new CookieService(HttpContext);
            var exp = DateTimeOffset.UtcNow.AddMinutes(TokenStatics.ExpirationDayRefreshCookie); // TODO: change AddMinutes to AddDays
            var refreshToken = Guid.NewGuid();
            cookieService.GenerateCookie(TokenStatics.RefreshCookieName, EncryptionHelper.EncryptGuidWithPublicKey(refreshToken, TokenStatics.RefreshTokenKey),exp);

            // post into userRefreshToken Entity
            UserRefreshTokenService refreshTokenService = new UserRefreshTokenService(_context);
            refreshTokenService.PostUserRefreshToken(new RequestUserRefreshTokenServiceDto
            {
                UserId = user.Id,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = req.UserAgent,
                Exp = exp.UtcDateTime, // Convert DateTimeOffset to DateTime (UTC time)
                Token = refreshToken.ToString(),
            });

            ///////////////////////////////////////////////////////////////
            ///                      AccessToken
            ///////////////////////////////////////////////////////////////
            
            
            exp = DateTimeOffset.UtcNow.AddMinutes(TokenStatics.ExpirationDayAuthCookie);// TODO: change [AddMinutes] to [AddDays]
            // generate token
            TokenAccessService tokenService = new TokenAccessService();
            UserTokenDto userTokenDto = new UserTokenDto()
            {
                UserId = user.Id.ToString(),
                Role = user.Role,
                Exp = exp.UtcDateTime, // Convert DateTimeOffset to DateTime (UTC time)
            };
            string token = tokenService.GenerateToken(userTokenDto, TokenStatics.AccessTokenKey);
            // generate cookie
            var checkCookie = cookieService.GenerateCookie(TokenStatics.AuthCookieName, token, exp);


            ///////////////////////////////////////////////////////////////
            ///                     Returned Value
            ///////////////////////////////////////////////////////////////
            return Json(new { token = checkCookie });

        }
        public IActionResult SignOut()
        {
            // 1. Remove the auth & refresh cookie
            Response.Cookies.Delete(TokenStatics.AuthCookieName);
            Response.Cookies.Delete(TokenStatics.RefreshCookieName);

            // 2. Clear claims identity (optional)
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // 3. Redirect to login/home page
            return RedirectToAction(TokenStatics.DestinationActionAfterLogout, TokenStatics.DestinationControllerAfterLogout);
        }
    }
}